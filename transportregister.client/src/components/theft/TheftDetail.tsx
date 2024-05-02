import { Component } from "react";
import ITheftDetail from "../interfaces/ITheftDetail";
import { Alert, Button, Col, Row } from "reactstrap";
import { IconButton } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import DetailIcon from '@mui/icons-material/VisibilityOutlined';
import { Link } from 'react-router-dom';
import { formatDateTime } from "../../common/DateFormatter";


interface ITheftDetailProps {
  theftDetail: ITheftDetail | null;
  showButtons: boolean;
}

export class TheftDetail extends Component<object, ITheftDetailProps> {
  constructor(props: object) {
    super(props);
    this.state = {
      theftDetail: null,
      showButtons: false
    };
  }

  async handleFound() {
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
        this.setState({ showButtons: true });
        //document.getElementById("editButton")?.classList.add("hidden");
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
                    <dt>Stav:</dt>
                    {theftDetail.isFound ?
                      <dd className="yes"><b>Nalezeno</b></dd>
                      :
                      <dd className="no"><b>Hledá se</b></dd>
                    }
                  </Col>
                  <Col>
                    <dt>Nalezeno:</dt>
                    {theftDetail.isFound ?
                      <dd className="yes">{formatDateTime(theftDetail.foundOn!)}</dd>
                      :
                      <dd className="no"><b>Nenalezeno</b></dd>
                    }
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
            {this.state.showButtons ?
              (
                <Row>
                  <hr />
                  <Col className="rightSide pe-0">
                    <Button color="success" className="me-2" onClick={this.handleFound}>Nahlásit nález</Button>
                  </Col>
                </Row>
              )
                :
              (
                <Row>
                  <hr />
                  <Col>
                    <dt className="mb-1">Nález nahlásil:</dt>
                    {/*<Link to={`/user/${theftDetail.official?.id}`}>*/}
                    {/*  <dd>{offenceDetail.official?.fullName}</dd>*/}
                    {/*</Link>*/}
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
