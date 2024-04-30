import { Component } from 'react';
import { Button, Col, Form, FormGroup, Input, Label, Row } from 'reactstrap';
export class VehicleCreate extends Component<object> {
  constructor(props: object) {
    super(props);
  }
  handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    const formData = {
      vin: (document.getElementById("vin") as HTMLInputElement).value,
      manufacturer: (document.getElementById("manufacturer") as HTMLInputElement).value,
      model: (document.getElementById("model") as HTMLInputElement).value,
      horsepowerKW: (document.getElementById("horsepowerKW") as HTMLInputElement).value,
      engineVolumeCM3: (document.getElementById("engineVolumeCM3") as HTMLInputElement).value,
      manufacturedYear: (document.getElementById("manufacturedYear") as HTMLInputElement).value,
      color: (document.getElementById("color") as HTMLInputElement).value,
      lengthCM: (document.getElementById("lengthCM") as HTMLInputElement).value,
      widthCM: (document.getElementById("widthCM") as HTMLInputElement).value,
      heightCM: (document.getElementById("heightCM") as HTMLInputElement).value,
      loadCapacityKG: (document.getElementById("loadCapacityKG") as HTMLInputElement).value
    };

    // Call your API endpoint with formData
    console.log(formData);

    // Reset form after submission
    event.currentTarget.reset();
  }

  render() {
    return (
      <div>
        <Row>
          <Col>
            <h4>Zaregistrovat vozidlo</h4>
          </Col>
        </Row>
        <Row>
          <Form className="container vehicelRegForm" onSubmit={(e) => this.handleSubmit(e)}>
            <FormGroup>
              <Label for="vin">VIN:</Label>
              <Input type="text" id="vin" required />
            </FormGroup>

            <FormGroup>
              <Label for="manufacturer">Výrobce:</Label>
              <Input type="text" id="manufacturer" required />
            </FormGroup>

            <FormGroup>
              <Label for="model">Model:</Label>
              <Input type="text" id="model" required />
            </FormGroup>

            <FormGroup>
              <Label for="horsepowerKW">Koňská síla (KW):</Label>
              <Input type="number" id="horsepowerKW" required />
            </FormGroup>

            <FormGroup>
              <Label for="engineVolumeCM3">Objem motoru (cm3):</Label>
              <Input type="number" id="engineVolumeCM3" required />
            </FormGroup>

            <FormGroup>
              <Label for="manufacturedYear">Rok výroby:</Label>
              <Input type="number" id="manufacturedYear" required />
            </FormGroup>


            <FormGroup>
              <Label for="lengthCM">Délka (cm):</Label>
              <Input type="number" id="lengthCM" required />
            </FormGroup>

            <FormGroup>
              <Label for="widthCM">Šírka (cm):</Label>
              <Input type="number" id="widthCM" required />
            </FormGroup>

            <FormGroup>
              <Label for="heightCM">Výška (cm):</Label>
              <Input type="number" id="heightCM" required />
            </FormGroup>


            <FormGroup>
              <Label for="color">Barva:</Label>
              <Input type="text" id="color" required />
            </FormGroup>

            <FormGroup>
              <Label for="loadCapacityKG">Nosnost (kg):</Label>
              <Input type="number" id="loadCapacityKG" required />
            </FormGroup>

            <Button type="submit">Zaregistrovat</Button>
          </Form>
        </Row>
      </div>
    );
  }
}

export default VehicleCreate;
