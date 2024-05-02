import { Component } from "react";
import ITheftDetail from "../interfaces/ITheftDetail";
import { Alert, Col, Row } from "reactstrap";
import { IconButton } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import DetailIcon from '@mui/icons-material/VisibilityOutlined';
import { Link } from 'react-router-dom';
import { formatDateTime } from "../../common/DateFormatter";


interface ITheftDetailProps {
  //theftId: number;
  theftDetail: ITheftDetail | null;
}

export class TheftDetail extends Component<object, ITheftDetailProps> {
  constructor(props: object) {
    super(props);
    this.state = {
      theftDetail: null,
    };
  }

  async populateTheftData() {
    const urlSplitted = window.location.pathname.split('/');
    const id = urlSplitted[2];
    const apiUrl = `/api/Theft/GetTheftById/${id}`;

    try {
      const response = await fetch(apiUrl); //TODO

      if (!response.ok) {
        console.error(response.statusText);
        return;
      }

      const data: ITheftDetail = await response.json();
      this.setState({ theftDetail: data });
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
                  {/*TODO ->errors in dev mode in browser console*/}
                  <Col id="editButton">
                    {/*<Tooltip title="Upravit přestupek">*/}
                    <IconButton color="primary" size="large" onClick={this.handleEditButton}>
                      <EditIcon fontSize="inherit" />
                    </IconButton>
                    {/*</Tooltip>*/}
                  </Col>
                  <Col className="hidden" id="saveButton">
                    {/*<Tooltip title="Uložit úpravy">*/}
                    <IconButton color="primary" size="large" onClick={this.handleSaveButton}>
                      <SaveIcon />
                    </IconButton>
                    {/*</Tooltip>*/}
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
            <Row>
              <hr />
              TODO BUTTON LOGIKA A LOGIKA UREDNIKA NA SCHVALENI?? PRIDAT BUTTON NAHLASIT NALEZ PRO POLICISTU A POTVRDIT NAVRACENI VOZIDLA PRO UREDNIKA?
            </Row>
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
