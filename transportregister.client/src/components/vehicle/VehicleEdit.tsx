import { Component } from 'react';
import { Link } from 'react-router-dom';
import { Alert, Button, Col, Row, } from 'reactstrap';
import { IBus, ICar, IMotorcycle, ITruck, IVehicleDetail } from '../interfaces/IVehicleDetail';

interface IVehicleDetailProps {
  vehicleDetail: IVehicleDetail | null;
}

export class VehicleEdit extends Component<object | IVehicleDetailProps> {
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
    const id = urlSplitted[3];

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
      this.setState({ vehicleDetail: parsedVehicle, vehicleId: id });
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
              <h4>Úprava vozidla</h4>
            </Col>
            <Col className="d-flex justify-content-end">
              <Button tag={Link} to={`/vehicle/${vehicleDetail.vehicleId}`} color="primary">
                Přejít na detail
              </Button>
            </Col>
          </Row>

          {/*<VehicleForm fetchedData={vehicleDetail} />*/}
        </>
      );

    return (
      <div className="container-fluid">
        {content}
      </div>
    );
  }
}

export default VehicleEdit;
