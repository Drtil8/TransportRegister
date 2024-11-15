﻿import { Component, ContextType } from 'react';
import { Link } from 'react-router-dom';
import { Alert, Button, Col, Row, Table } from 'reactstrap';
import { IBus, ICar, IMotorcycle, ITruck, IVehicleDetail } from '../interfaces/IVehicleDetail';
import { formatDate } from '../../common/DateFormatter';
import ILicensePlateHistory from '../interfaces/ILicensePlateHistory';
import AuthContext from '../../auth/AuthContext';
import OffenceReportVehicleModal from '../offence/OffenceReportVehicleModal';
import TheftReportModal from '../theft/TheftReportModal';
import IOffenceListSimple from '../interfaces/IOffenceListSimple';
import DetailIcon from '@mui/icons-material/VisibilityOutlined';

interface IVehicleDetailProps {
  vehicleDetail: IVehicleDetail | null;
  offences: IOffenceListSimple[];
}

export class VehicleDetail extends Component<object | IVehicleDetailProps> {
  static contextType = AuthContext;
  declare context: ContextType<typeof AuthContext>;
  constructor(props: object) {
    super(props);
    this.state = {
      vehicleDetail: null,
      offences: [],
    }
  }

  componentDidMount() {
    this.populateVehicleData();
  }

  async fetchOffences(id: number) {
    try {
      const response = await fetch(`/api/Persons/${id}/CommitedOffences`);
      if (!response.ok) {
        throw new Error(`Failed to load CommitedOffences.`);
      }
      const jsonOffenses = await response.json();
      let offList = jsonOffenses as IOffenceListSimple[];
      this.setState({ offences: offList });
    }
    catch (error) {
      console.error('Error fetching offenses data:', error);
    }
  }

  async populateVehicleData() {
    const urlSplitted = window.location.pathname.split('/');
    const id = urlSplitted[2];

    try {
      const response = await fetch(`/api/Vehicle/${id}`);
      if (!response.ok) {
        throw new Error(`Failed to load VehicleById.`);
      }
      const vehicle = await response.json();
      if (vehicle.imageBase64)
        vehicle.imageBase64 = "data:image/jpeg;base64," + vehicle.imageBase64;

      let parsedVehicle: IVehicleDetail;
      switch (vehicle.vehicleType) {
        case 'Car':
          parsedVehicle = vehicle as ICar;
          break;
        case 'Truck':
          parsedVehicle = vehicle as ITruck;
          break;
        case 'Motorcycle':
          parsedVehicle = vehicle as IMotorcycle;
          break;
        case 'Bus':
          parsedVehicle = vehicle as IBus;
          break;
        default:
          throw new Error(`Unknown vehicle type: ${vehicle.vehicleType}`);
      }
      this.setState({ vehicleDetail: parsedVehicle });
      this.fetchOffences(parsedVehicle.ownerId);
      console.log(vehicle.currentlyStolen);
    }
    catch (error) {
      console.error('Error fetching vehicle data:', error);
    }
  }

  render() {
    const { vehicleDetail, offences } = this.state as IVehicleDetailProps;
    const localizedVehicleTypeMap: { [key: string]: string } = {
      'Car': 'auta',
      'Truck': 'nákladního auta',
      'Motorcycle': 'motocyklu',
      'Bus': 'autobusu'
    };

    const licensePlatesTable = (
      <>
        {(vehicleDetail?.licensePlateHistory && vehicleDetail!.licensePlateHistory.length > 1) && (
          <>
            <h5>Historie SPZ</h5>
            <Table>
              <thead>
                <tr>
                  <th>SPZ</th>
                  <th>Datum změny</th>
                </tr>
              </thead>
              <tbody>
                {vehicleDetail!.licensePlateHistory.map((licensePlate: ILicensePlateHistory) => (
                  <tr key={licensePlate.licensePlateHistoryId}>
                    <td>{licensePlate.licensePlate}</td>
                    <td>{formatDate(licensePlate.changedOn)}</td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </>
        )}
      </>
    );

    const offencesTable = (
      <>
        <h5>Historie přestupků</h5>
        {(offences.length === 0) && (
          <p>Vozidlo nemá žádné přestupky.</p>
        )}
        {(offences.length > 1) && (
          <Table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Přestupek</th>
                <th>Body</th>
                <th>Pokuta</th>
                <th>Zobrazit</th>
              </tr>
            </thead>
            <tbody>
              {offences.map((offence: IOffenceListSimple) => (
                <tr key={offence.offenceId}>
                  <td>{offence.offenceId}</td>
                  <td>{offence.description}</td>
                  <td>{offence.penaltyPoints}</td>
                  <td>{offence.fineAmount} Kč</td>
                  <td>
                    <Link to={`/offence/${offence.offenceId}`}>
                      <DetailIcon />
                    </Link>
                  </td>
                </tr>
              ))}
            </tbody>
          </Table>
        )}
      </>
    );

    const content = !vehicleDetail ?
      (
        <Alert color="danger">Vozidlo nebylo nalezeno.</Alert>
      )
      :
      (
        <>
          <Row className="mb-3">
            <Col xs="12" sm="6">
              <h4>Detail {localizedVehicleTypeMap[vehicleDetail.vehicleType]}
                {vehicleDetail.currentlyStolen && (<b className="no"> - Aktuálně ukradené</b>)}</h4>
            </Col>
            {this.context?.isOfficial && (
              <Col className="d-flex justify-content-end">
                {vehicleDetail.currentlyStolen && (
                  <Button tag={Link} to={`/theft/${vehicleDetail.currentlyStolenId}`} color="secondary" className="me-2">
                    Zobrazit detail krádeže
                  </Button>
                )}
                <Button tag={Link} to={`/vehicle/edit/${vehicleDetail.vehicleId}`} color="primary">
                  Upravit vozidlo
                </Button>
              </Col>
            )}
            {this.context?.isOfficer && !vehicleDetail.currentlyStolen && (
              <Col className="d-flex justify-content-center justify-content-sm-end me-sm-2" >
                <OffenceReportVehicleModal vehicleDetail={vehicleDetail} />
                <TheftReportModal vehicleDetail={vehicleDetail}></TheftReportModal>
              </Col>
            )}
          </Row>

          <Row>
            <Col xs="10">
              <Row>
                <dl>
                  <Row>
                    {vehicleDetail.imageBase64 && (
                      <Col xs="6">
                        <img
                          src={`${vehicleDetail.imageBase64}`}
                          alt="Vozidlo"
                          className="vehicleImage" />
                      </Col>
                    )}
                    <Col>
                      <Row className="mb-3">
                        <dt>VIN:</dt>
                        <dd>{vehicleDetail.vin}</dd>
                      </Row>
                      <Row className="mb-3">
                        <dt>SPZ:</dt>
                        <dd>{vehicleDetail.currentLicensePlate}</dd>
                      </Row>
                      <Row className="mb-3">
                        <dt>Model:</dt>
                        <dd>{`${vehicleDetail.manufacturer} ${vehicleDetail.model}, ${vehicleDetail.manufacturedYear}`}</dd>
                      </Row>
                      <Row className="mb-3">
                        <dt>Barva:</dt>
                        <dd>{vehicleDetail.color}</dd>
                      </Row>
                      <Row>
                        <dt>Vlastník:</dt>
                        <dd>
                          <Link to={`/driver/${vehicleDetail.ownerId}`}>
                            {vehicleDetail.ownerFullName}
                          </Link>
                        </dd>
                      </Row>
                    </Col>
                  </Row>

                  <dt>Dodatečné informace:</dt>
                  <Row id="basicVehicleInfo">
                    <Col>
                      <dt>Výkon:</dt>
                      <dd>{`${vehicleDetail.horsepowerKW} kW`}</dd>

                      <dt>Objem motoru:</dt>
                      <dd>{`${vehicleDetail.engineVolumeCM3} cm`}<sup>3</sup></dd>
                    </Col>
                    <Col>
                      <dt>Rozměry (d x š x v):</dt>
                      <dd>{`${vehicleDetail.lengthCM} x ${vehicleDetail.widthCM} x ${vehicleDetail.heightCM} cm`}</dd>

                      <dt>Nosnost:</dt>
                      <dd>{`${vehicleDetail.loadCapacityKG} kg`}</dd>
                    </Col>
                  </Row>

                  <Row id="specificVehicleInfo">
                    {vehicleDetail.vehicleType === 'Car' && (
                      <>
                        <dt>Počet dveří:</dt>
                        <dd>{(vehicleDetail as ICar).numberOfDoors}</dd>
                      </>
                    )}
                    {vehicleDetail.vehicleType === 'Motorcycle' && (
                      <>
                        <dt>Omezení:</dt>
                        <dd>{(vehicleDetail as IMotorcycle).constraints}</dd>
                      </>
                    )}
                    {vehicleDetail.vehicleType === 'Bus' && (
                      <>
                        <dt>Kapacita (sezení + stání):</dt>
                        <dd>{`${(vehicleDetail as IBus).seatCapacity} + ${(vehicleDetail as IBus).standingCapacity}`}</dd>
                      </>
                    )}
                    {/*{vehicleDetail.vehicleType === 'Truck' && (*/}
                    {/*)}*/}
                  </Row>
                </dl>
              </Row>
            </Col>
          </Row>

          {licensePlatesTable}

          {offencesTable}

          <Row>
            <Col className="col-3">
              <dt>Naposledy upraveno úředníkem:</dt>
              <dd>
                <Link to={`/user/${vehicleDetail.officialId}`}>
                  {vehicleDetail.officialFullName}
                </Link>
              </dd>
            </Col>
          </Row>
        </>
      );

    return (
      <div className="container-fluid">
        {content}
      </div>
    );
  }
}

export default VehicleDetail;
