import { Component } from 'react';
import { Button, Col, Form, FormGroup, Input, Label, Row, } from 'reactstrap';

export class VehicleDetail extends Component<object> {
  constructor(props: object) {
    super(props);
  }

  // todo later load Interface IVehicleDetail
  vehicleId = 1;

  async uploadVehicleFile(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    let formData = new FormData(document.getElementById("vehicleFileUpload") as HTMLFormElement);
    formData.append("vehicleId", String(this.vehicleId));
    try {
      const response = await fetch(`/api/Vehicle/${this.vehicleId}/FileUpload`, {
        method: 'POST',
        headers: {
          'Content-Type': 'multipart/form-data'
        },
        body: formData
      });

      if (!response.ok) {
        throw new Error('Něco se pokazilo! Zkuste to prosím znovu.');
      }
      else {
        console.log('Image uploaded successfully');
      }
    }
    catch (error) {
      console.error('Failed to upload image: ' + error);
    }
  }

  render() {
    return (
      <>
        <Row>
          <Col>
            <h4>Detail vozidla</h4>
          </Col>
          <Col className="d-flex justify-content-end">
            {/*<CreateVehicleModal />*/}
          </Col>
        </Row>

        <Form id="vehicleFileUpload"
          method="post"
          onSubmit={(e) => this.uploadVehicleFile(e)}
          encType="multipart/form-data">
          {/*todo or try in form action={`/api/Vehicle/${this.vehicleId}/FileUpload`}*/}
          <FormGroup>
            <Label for="vehicleImage">
              Fotka vozidla
            </Label>
            <Input
              id="vehicleImage"
              name="file"
              type="file"
              accept="image/*" />
          </FormGroup>
          <FormGroup>
            <Label for="vehicleGallery">
              Galerie vozidla
            </Label>
            <Input
              id="vehicleGallery"
              name="file"
              type="file"
              accept="image/*"
              multiple />
          </FormGroup>
          <Button type="submit" className="btn btn-primary">Uložit</Button>
        </Form>
      </>
    );
  }
}

export default VehicleDetail;
