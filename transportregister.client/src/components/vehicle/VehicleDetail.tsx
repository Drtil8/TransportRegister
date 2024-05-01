import { Component } from 'react';
import { Link } from 'react-router-dom';
import { Alert, Button, Col, Row } from 'reactstrap';
import { IBus, ICar, IMotorcycle, ITruck, IVehicleDetail } from '../interfaces/IVehicleDetail';

interface IVehicleDetailProps {
  vehicleDetail: IVehicleDetail | null;
}

export class VehicleDetail extends Component<object | IVehicleDetailProps> {
  constructor(props: object) {
    super(props);
    this.state = {
      vehicleDetail: null
    }
  }

  componentDidMount() {
    this.populateVehicleData();
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
        vehicle.imageBase64 = "data:image/jpeg;base64," + vehicle.imageBase64;    // todo set proper image extension

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
    }
    catch (error) {
      console.error('Error fetching vehicle data:', error);
    }
  }

  render() {
    const { vehicleDetail } = this.state as IVehicleDetailProps;
    const localizedVehicleTypeMap: { [key: string]: string } = {
      'Car': 'auta',
      'Truck': 'nákladního auta',
      'Motorcycle': 'motocyklu',
      'Bus': 'autobusu'
    };

    const content = !vehicleDetail ?
      (
        <Alert color="danger">Vozidlo nebylo nalezeno.</Alert>
      )
      :
      (
        <>
          <Row className="mb-3">
            <Col>
              <h4>Detail {localizedVehicleTypeMap[vehicleDetail.vehicleType]}</h4>
            </Col>
            <Col className="d-flex justify-content-end">
              <Button tag={Link} to={`/vehicle/edit/${vehicleDetail.vehicleId}`} color="primary">
                Upravit vozidlo
              </Button>
            </Col>
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
                    </Col>
                  </Row>

                  {/* todo should be collapsable */}
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
