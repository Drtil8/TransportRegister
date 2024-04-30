import { Component } from 'react';
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
      console.log(parsedVehicle);
    }
    catch (error) {
      console.error('Error fetching vehicle data:', error);
    }
  }

  render() {
    const { vehicleDetail } = this.state as IVehicleDetailProps;
    const content = !vehicleDetail ?
      (
        <Alert color="danger">Vozidlo nebylo nalezeno.</Alert>
      )
      :
      (
        <>
          <Row className="mb-3">
            <Col>
              <h4>Detail vozidla</h4>
            </Col>
            <Col className="d-flex justify-content-end">
              <Button color="success" href={`/vehicle/edit/${vehicleDetail.vehicleId}`}>
                Upravit vozidlo
              </Button>
            </Col>
          </Row>

          <Row>
            <Col xs="10">
              <Row>
                <dl>
                  <Row>
                    <dt>VIN:</dt>
                    <dd>{vehicleDetail.vin}</dd>
                  </Row>
                  <Row>
                    <dt>SPZ:</dt>
                    <dd>{vehicleDetail.currentLicensePlate}</dd>
                  </Row>
                  <Row>
                    <dt>Model:</dt>
                    <dd>{`${vehicleDetail.manufacturer} ${vehicleDetail.model}`}</dd>
                  </Row>
                  <Row>
                    <dt>Rok výroby:</dt>
                    <dd>{vehicleDetail.manufacturedYear}</dd>
                  </Row>
                  <Row>
                    <dt>Barva:</dt>
                    <dd>{vehicleDetail.color}</dd>
                  </Row>

                  {/*todo*/}
                  <dt>Dodatečné informace:</dt>
                  <Row>
                    <dt>Obecné:</dt>
                    <dd>{vehicleDetail.horsepowerKW}</dd>
                    <dd>{vehicleDetail.engineVolumeCM3}</dd>
                    <dd>{`${vehicleDetail.lengthCM} X ${vehicleDetail.widthCM} X ${vehicleDetail.heightCM}`}</dd>
                    <dd>{vehicleDetail.loadCapacityKG}</dd>

                    <dt>Specifické pro typ vozidla - {vehicleDetail.vehicleType}:</dt>
                    {vehicleDetail.vehicleType === 'Car' && (
                      <dd>{(vehicleDetail as ICar).numberOfDoors}</dd>
                    )}
                    {vehicleDetail.vehicleType === 'Motorcycle' && (
                      <dd>{(vehicleDetail as IMotorcycle).constraints}</dd>
                    )}
                    {vehicleDetail.vehicleType === 'Bus' && (
                      <>
                        <dd>{(vehicleDetail as IBus).seatCapacity}</dd>
                        <dd>{(vehicleDetail as IBus).standingCapacity}</dd>
                      </>
                    )}
                    {/*{vehicleDetail.vehicleType === 'Truck' && (*/}
                    {/*)}*/}

                    {vehicleDetail.imageBase64 !== null && (
                      <Col xs="8">
                        <dt>Fotka vozidla:</dt>
                        <img
                          src={`${vehicleDetail.imageBase64}`}
                          alt="Vozidlo"
                          className="vehicleImage" />
                      </Col>
                    )}
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
