import { Component } from 'react';
import { Button, Col, Form, FormGroup, Input, Label, Row } from 'reactstrap';

interface IVehicleCreateProps {
  selectedVehicleType: string;
  image: string | null;
}

export class VehicleCreate extends Component<object, IVehicleCreateProps> {
  constructor(props: object) {
    super(props);
    this.state = {
      selectedVehicleType: 'Car',
      image: null,
    }
  }


  async handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    let formData = new FormData(document.getElementById("vehicleCreateForm") as HTMLFormElement);
    let vehicleType = formData.get('vehicleType');
    let additionalFields = {};
    if (vehicleType === 'Car')
      additionalFields = {
        numberOfDoors: formData.get('numberOfDoors') || 0
      };
    else if (vehicleType === 'Motorcycle')
      additionalFields = {
        constraints: formData.get('constraints')
      };
    else if (vehicleType === 'Bus')
      additionalFields = {
        seatCapacity: formData.get('seatCapacity') || 0,
        standingCapacity: formData.get('standingCapacity') || 0
      };


    let imageBase64 = this.state.image;
    if (imageBase64) {
      imageBase64 = imageBase64.replace(/^data:image\/[a-z]+;base64,/, "");
    }

    let params = {
      ownerId: 1, // todo get from user
      vehicleType: vehicleType,
      VIN: formData.get('VIN'),
      currentLicensePlate: formData.get('SPZ'),
      manufacturer: formData.get('manufacturer'),
      model: formData.get('model'),
      manufacturedYear: formData.get('manufacturedYear') || 0,
      horsepowerKW: formData.get('horsepowerKW') || 0,
      engineVolumeCM3: formData.get('engineVolumeCM3') || 0,
      loadCapacityKG: formData.get('loadCapacityKG') || 0,
      lengthCM: formData.get('lengthCM') || 0,
      widthCM: formData.get('widthCM') || 0,
      heightCM: formData.get('heightCM') || 0,
      color: formData.get('color'),
      imageBase64: imageBase64,
      ...additionalFields,
    };
    console.log(params);

    try {
      const response = await fetch(`/api/Vehicle/SaveVehicle`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(params)
      });

      if (!response.ok) {
        console.error("Create vehicle failed");
      }
    }
    catch (error) {
      console.error('Create vehicle failed: ' + error);
    }
  }

  handleVehicleTypeChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    this.setState({ selectedVehicleType: event.target.value });
  };

  handleImageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files && event.target.files[0];
    if (!file)
      return;
    const reader = new FileReader();
    reader.onloadend = () => {
      const { selectedVehicleType } = this.state as IVehicleCreateProps;
      const imageBase64 = reader.result as string;
      this.setState({ selectedVehicleType, image: imageBase64 });
    };
    reader.readAsDataURL(file);
  };

  render() {
    const { selectedVehicleType } = this.state as IVehicleCreateProps;

    return (
      <>
        <Row>
          <Col>
            <h4>Registrace vozidla</h4>
          </Col>
        </Row>
        <Form
          id="vehicleCreateForm"
          encType="multipart/form-data"
          onSubmit={(e) => this.handleSubmit(e)}>
          <Row>
            <Col>
              <FormGroup>
                <Label for="VIN">VIN:</Label>
                <Input type="text" name="VIN" required />
              </FormGroup>
            </Col>
            <Col>
              <FormGroup>
                <Label for="SPZ">SPZ:</Label>
                <Input type="text" name="SPZ" required />
              </FormGroup>
            </Col>
          </Row>

          <Row>
            <Col>
              <FormGroup>
                <Label for="manufacturer">Výrobce:</Label>
                <Input type="text" name="manufacturer" required />
              </FormGroup>
            </Col>
            <Col>
              <FormGroup>
                <Label for="model">Model:</Label>
                <Input type="text" name="model" required />
              </FormGroup>
            </Col>
          </Row>

          <Row>
            <Col>
              <FormGroup>
                <Label for="horsepowerKW">Výkon (kW):</Label>
                <Input type="number" name="horsepowerKW" min="0" />
              </FormGroup>
            </Col>
            <Col>
              <FormGroup>
                <Label for="engineVolumeCM3">Objem motoru (cm<sup>3</sup>):</Label>
                <Input type="number" name="engineVolumeCM3" min="0" />
              </FormGroup>
            </Col>
          </Row>

          <Row>
            <Col>
              <FormGroup>
                <Label for="manufacturedYear">Rok výroby:</Label>
                <Input type="number" name="manufacturedYear" min="1900" />
              </FormGroup>
            </Col>
            <Col>
              <FormGroup>
                {/* todo číselník barev - select + možnost přidat novou barvu */}
                <Label for="color">Barva:</Label>
                <Input type="text" name="color" />
              </FormGroup>
            </Col>
            <Col>
              <FormGroup>
                <Label for="loadCapacityKG">Nosnost (kg):</Label>
                <Input type="number" name="loadCapacityKG" min="0" />
              </FormGroup>
            </Col>
          </Row>

          <Row>
            <Label for="lengthCM">Rozměry (d x š x v) cm:</Label>
            <Col>
              <Input type="number" name="lengthCM" min="0" />
            </Col>
            <Col>
              <Input type="number" name="widthCM" min="0" />
            </Col>
            <Col>
              <Input type="number" id="heightCM" min="0" />
            </Col>
          </Row>

          <Row>
            <Col xs="6">
              <FormGroup>
                <Label for="vehicleType">Typ vozidla:</Label>
                <Input
                  type="select"
                  name="vehicleType"
                  defaultValue={selectedVehicleType}
                  onChange={this.handleVehicleTypeChange}
                  required>
                  <option key="Car" value="Car">Auto</option>
                  <option key="Truck" value="Truck">Nákladní auto</option>
                  <option key="Motorcycle" value="Motorcycle">Motorka</option>
                  <option key="Bus" value="Bus">Autobus</option>
                </Input>
              </FormGroup>
            </Col>
          </Row>

          {selectedVehicleType === 'Car' && (
            <Row id="carInputs">
              <Col>
                <FormGroup>
                  <Label for="numberOfDoors">Počet dveří:</Label>
                  <Input type="number" name="numberOfDoors" min="1" defaultValue="5" />
                </FormGroup>
              </Col>
            </Row>
          )}
          {selectedVehicleType === 'Motorcycle' && (
            <Row id="motorcycleInputs">
              <Col>
                <FormGroup>
                  <Label for="constraints">Omezení:</Label>
                  <Input type="text" name="constraints" />
                </FormGroup>
              </Col>
            </Row>
          )}
          {selectedVehicleType === 'Bus' && (
            <Row id="busInputs">
              <Label for="constraints">Kapacita (sezení + stání):</Label>
              <Col>
                <Input type="number" name="seatCapacity" min="0" />
              </Col>
              <Col>
                <Input type="number" name="standingCapacity" min="0" />
              </Col>
            </Row>
          )}
          {/*{selectedVehicleType === 'Truck' && (*/}
          {/*)}*/}

          <Row>
            <FormGroup>
              <Label for="vehicleImage">
                Fotka vozidla
              </Label>
              {this.state.image && (
                <Row className="mb-2">
                  <Col xs="6">
                    <img
                      src={`${this.state.image}`}
                      alt="Vozidlo"
                      className="vehicleImage" />
                  </Col>
                </Row>
              )}
              <Input
                id="vehicleImage"
                name="ImageBase64"
                type="file"
                accept="image/*"
                onChange={this.handleImageChange} />
            </FormGroup>
          </Row>

          <Row className="my-3">
            <Col className="d-flex justify-content-center">
              <Button type="submit" size="lg" color="primary">Zaregistrovat</Button>
            </Col>
          </Row>
        </Form>
      </>
    );
  }
}

export default VehicleCreate;
