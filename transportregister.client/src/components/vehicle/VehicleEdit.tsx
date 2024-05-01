import { ChangeEvent, Component } from 'react';
import { Link } from 'react-router-dom';
import { Alert, Button, Col, Form, FormGroup, Input, Label, Row, } from 'reactstrap';
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

  async uploadVehicleFile(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const { vehicleDetail } = this.state as IVehicleDetailProps;
    let formData = new FormData(document.getElementById("vehicleFileUpload") as HTMLFormElement);
    formData.append("vehicleId", String(vehicleDetail?.vehicleId));
    try {
      const response = await fetch(`/api/Vehicle/${vehicleDetail?.vehicleId}/UploadImage`, {
        method: 'POST',
        body: formData
      });

      if (response.ok) {
        alert('Image uploaded successfully');
      }
      else {
        console.error("File not found or not of type File");
      }
    }
    catch (error) {
      console.error('Failed to upload image: ' + error);
    }
  }

  handleImageChange = (event: ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files && event.target.files[0];
    if (!file)
      return;
    const reader = new FileReader();
    reader.onloadend = () => {
      const { vehicleDetail } = this.state as IVehicleDetailProps;
      const imageBase64 = reader.result as string;
      const updatedVehicleDetail = { ...vehicleDetail, imageBase64 };
      this.setState({ vehicleDetail: updatedVehicleDetail });
    };
    reader.readAsDataURL(file);
  };

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

          {vehicleDetail.imageBase64 && (
            <Row>
              <Col xs="8">
                <dt>Fotka vozidla:</dt>
                <img
                  src={`${vehicleDetail.imageBase64}`}
                  alt="Vozidlo"
                  className="vehicleImage" />
              </Col>
            </Row>
          )}

          <Form id="vehicleFileUpload"
            method="post"
            onSubmit={(e) => this.uploadVehicleFile(e)}
            encType="multipart/form-data">
            <FormGroup>
              <Label for="vehicleImage">
                Fotka vozidla
              </Label>
              <Input
                id="vehicleImage"
                name="file"
                type="file"
                accept="image/*"
                onChange={this.handleImageChange} />
            </FormGroup>
            {/*todo add galery somewhere*/}
            {/*<FormGroup>*/}
            {/*  <Label for="vehicleGallery">*/}
            {/*    Galerie vozidla*/}
            {/*  </Label>*/}
            {/*  <Input*/}
            {/*    id="vehicleGallery"*/}
            {/*    name="file"*/}
            {/*    type="file"*/}
            {/*    accept="image/*"*/}
            {/*    multiple />*/}
            {/*</FormGroup>*/}
            <Button type="submit" color="primary">Uložit</Button>
          </Form>
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
