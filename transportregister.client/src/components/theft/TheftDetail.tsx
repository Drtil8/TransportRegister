import { Component, ContextType } from "react";
import ITheftDetail from "../interfaces/ITheftDetail";
import { Alert, Button, Col, Row } from "reactstrap";
import { IconButton } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import DetailIcon from '@mui/icons-material/VisibilityOutlined';
import { Link } from 'react-router-dom';
import { formatDateTime } from "../../common/DateFormatter";
import AuthContext from '../../auth/AuthContext';


interface ITheftDetailProps {
  theftDetail: ITheftDetail | null;
  showOfficerButtons: boolean;
  showOfficialButtons: boolean;
}

export class TheftDetail extends Component<object, ITheftDetailProps> {
  static contextType = AuthContext;
  declare context: ContextType<typeof AuthContext>;

  constructor(props: object) {
    super(props);
    this.state = {
      theftDetail: null,
      showOfficerButtons: false,
      showOfficialButtons: false
    };
    this.handleFound = this.handleFound.bind(this);
    this.handleReturn = this.handleReturn.bind(this);
  }

  async handleFound() {
    console.log(this.state.theftDetail);
    try {
      const response = await fetch(`/api/Theft/ReportTheftDiscovery/${this.state.theftDetail?.theftId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        }
      });

      if (!response.ok) {
        throw new Error(response.statusText);
      }

      this.setState({ showOfficerButtons: false });
      this.populateTheftData();
    }
    catch (error) {
      console.error(error);
    }
  }

  async handleReturn() {
    try {
      const response = await fetch(`/api/Theft/ReportTheftReturn/${this.state.theftDetail?.theftId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        }
      });

      if (!response.ok) {
        throw new Error(response.statusText);
      }

      this.setState({ showOfficialButtons: false });
      this.populateTheftData();
    }
    catch (error) {
      console.error(error);
    }
  }

  async populateTheftData() {
    const urlSplitted = window.location.pathname.split('/');
    const id = urlSplitted[2];
    const apiUrl = `/api/Theft/GetTheftById/${id}`;

    try {
      const response = await fetch(apiUrl);

      if (!response.ok) {
        console.error(response.statusText);
        return;
      }

      const data: ITheftDetail = await response.json();
      this.setState({ theftDetail: data });
      if (!data.isFound) {
        this.setState({ showOfficerButtons: true });
      }

      if (data.isFound && !data.isReturned) {
        this.setState({ showOfficialButtons: true });
      }

    } catch (error) {
      console.error(error);
    }
  }

  componentDidMount() {
    this.populateTheftData();
  }

  render() {
    const { theftDetail } = this.state;

    const contetnt = !theftDetail ?
      (
        <Alert color="danger"> Přestupek nebyl nalezen. </Alert>
      )
      :
      (
        <Row>
          <Col>
            <Row>
              <Col>
                <h4 className="pt-3">Detail krádeže - K.{theftDetail.theftId}</h4>
              </Col>
              <Col className="rightSide col-2">
                <Row>
                  <Col id="editButton">
                    {/*<IconButton color="primary" size="large" onClick={this.handleEditButton}>*/}
                    <IconButton color="primary" size="large">
                      <EditIcon fontSize="inherit" />
                    </IconButton>
                  </Col>
                  <Col className="hidden" id="saveButton">
                    <IconButton color="primary" size="large">
                      {/*<IconButton color="primary" size="large" onClick={this.handleSaveButton}>*/}
                      <SaveIcon />
                    </IconButton>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <dl>
                <Row>
                  <dt>Popis:</dt>
                  <dd><textarea readOnly value={theftDetail.description ? theftDetail.description : ""} className="form-control" /></dd>
                </Row>
                <Row>
                  <Col>
                    <dt>Ukradeno:</dt>
                    <dd>{formatDateTime(theftDetail.stolenOn)}</dd>
                  </Col>
                  <Col>
                    <dt>Nahlášeno:</dt>
                    <dd>{formatDateTime(theftDetail.reportedOn)}</dd>
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <dt>Nalezeno:</dt>
                    {theftDetail.isFound ?
                      <dd className="yes">{formatDateTime(theftDetail.foundOn!)}</dd>
                      :
                      <dd className="no"><b>Nenalezeno</b></dd>
                    }
                  </Col>
                  <Col>
                    <dt>Navráceno:</dt>
                    {theftDetail.isReturned ?
                      <dd className="yes">{formatDateTime(theftDetail.returnedOn!)}</dd>
                      :
                      <dd className="no"><b>Nenavráceno</b></dd>
                    }
                  </Col>
                </Row>
                {theftDetail.address !== "" && (
                  <Row>
                    <Col>
                      <dt>Ukradeno na adrese:</dt>
                      <dd>{theftDetail.address}</dd>
                    </Col>
                  </Row>
                )}
                <Row>
                  <Col>
                    <dt>Stav:</dt>
                    {theftDetail.isFound && theftDetail.isReturned ?
                      <dd className="yes"><b>Navráceno</b></dd>
                      :
                      (theftDetail.isFound && !theftDetail.isReturned ?
                        <dd className="workedOn"><b>Čeká na navrácení</b></dd>
                        :
                        <dd className="no"><b>Hledá se</b></dd>
                      )
                    }
                  </Col>
                  <Col>
                    <dt>Krádež nahlásil:</dt>
                    <dd>
                      <Link to={`/user/${theftDetail.officerReported?.id}`}>
                        {theftDetail.officerReported?.fullName}
                      </Link>
                    </dd>
                  </Col>
                </Row>
                <Row>
                  <hr />
                  <Row>
                    <Col>
                      <h4>Vozidlo</h4>
                    </Col>
                    <Col className="rightSide col-2">
                      <Link to={`/vehicle/${theftDetail.vehicleId}`}>
                        <DetailIcon />
                      </Link>
                    </Col>
                  </Row>
                  <Row>
                    <Col>
                      <dt>SPZ:</dt>
                      <dd>{theftDetail.stolenVehicle.licensePlate}</dd>
                    </Col>
                    <Col>
                      <dt>VIN:</dt>
                      <dd>{theftDetail.stolenVehicle.vin}</dd>
                    </Col>
                  </Row>
                  <Row>
                    <Col>
                      <dt>Značka:</dt>
                      <dd>{theftDetail.stolenVehicle.manufacturer}</dd>
                    </Col>
                    <Col>
                      <dt>Model:</dt>
                      <dd>{theftDetail.stolenVehicle.model}</dd>
                    </Col>
                  </Row>
                  <Row>
                    <Col>
                      <dt>Barva:</dt>
                      <dd>{theftDetail.stolenVehicle.color}</dd>
                    </Col>
                    <Col>
                      <dt>Rok výroby:</dt>
                      <dd>{theftDetail.stolenVehicle.manufacturedYear}</dd>
                    </Col>
                  </Row>
                  <Row>
                    <Col>
                      <dt>Typ:</dt>
                      <dd>{theftDetail.stolenVehicle.vehicleType}</dd>
                    </Col>
                    <Col>
                      <dt>Vlastník:</dt>
                      <dd>{theftDetail.stolenVehicle.ownerFullName}</dd>
                    </Col>
                  </Row>
                </Row>
              </dl>
            </Row>
            {this.state.showOfficerButtons && this.context?.isOfficer &&
              (
                <Row>
                  <hr />
                  <Col className="rightSide pe-0">
                    <Button color="success" className="me-2" onClick={this.handleFound}>Nahlásit nález</Button>
                  </Col>
                </Row>
              )}
            {this.state.showOfficialButtons && this.context?.isOfficial &&
              (
                <Row>
                  <hr />
                  <Col className="rightSide pe-0">
                    <Button color="success" className="me-2" onClick={this.handleReturn}>Potvrdit navrácení vozidla</Button>
                  </Col>
                </Row>
              )}
            {!this.state.showOfficerButtons && !this.state.showOfficialButtons &&
              (
                <Row>
                  <hr />
                  <Col>
                    <dt className="mb-1">Nález nahlásil:</dt>
                    <dd>
                      <Link to={`/user/${theftDetail.officerFound?.id}`}>
                        {theftDetail.officerFound?.fullName}
                      </Link>
                    </dd>
                  </Col>
                  <Col>
                    <dt className="mb-1">Navrácení potvrdil:</dt>
                    <dd>
                      <Link to={`/user/${theftDetail.official?.id}`}>
                        {theftDetail.official?.fullName}
                      </Link>
                    </dd>
                  </Col>
                </Row>
              )}
          </Col>
        </Row>
      );

    return (
      <div className="container-fluid">
        {contetnt}
      </div>
    );
  }
}
