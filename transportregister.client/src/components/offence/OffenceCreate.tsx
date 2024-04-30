import { Component, FormEvent } from 'react';
import { Col, Form, FormGroup, Input, Label, Row } from 'reactstrap';
import OffenceReportDriverModal from './OffenceReportDriverModal';
import OffenceReportVehicleModal from './OffenceReportVehicleModal';
//import LocationPicker from '../location/LocationPicker';

class OffenceCreate extends Component {
  handleSubmit = (event: FormEvent) => {
    event.preventDefault();
    const data = new FormData(event.target as HTMLFormElement);
    // here I will send the data to the server
    console.log(data);
  };

  render() {
    return (
      <div className="container-fluid">
        <OffenceReportDriverModal></OffenceReportDriverModal>
        <OffenceReportVehicleModal></OffenceReportVehicleModal>
        <Row>
          <Col>
            <h4>Nahlásit přestupek</h4>
          </Col>
        </Row>
        <Row>
          <Col>
            <Form id="OffenceCreateForm" onSubmit={this.handleSubmit}>
              <Row>
                <FormGroup>
                  {/*TODO -> pridat moznost pridani noveho typu?*/}
                  <Label>Typ přestupku ?? :</Label>
                  <Input type="select" />
                </FormGroup>
              </Row>
              <Row>
                <FormGroup>
                  <Label>Popis:</Label>
                  <Input type="textarea" />
                </FormGroup>
              </Row>
              {/*<Row>*/}
              {/*  <FormGroup>*/}
              {/*    <Label>Datum a čas:</Label>*/}
              {/*    <Input type="datetime-local" />*/}
              {/*  </FormGroup>*/}
              {/*</Row>*/}
              <Row>
                <FormGroup>
                  <Label>Místo činu:</Label>
                  <Input></Input>
                  {/*<LocationPicker />*/}
                </FormGroup>
              </Row>
              <Row>
                <Col className="col col-5">
                  <FormGroup>
                    <Label>Trestné body</Label>
                    <Input type="number"></Input>
                  </FormGroup>
                </Col>
                <Col className="col col-5">
                  <Row>
                    <FormGroup>
                      <Label>Výše pokuty</Label>
                      <Input type="number" />
                    </FormGroup>
                  </Row>
                </Col>
                <Col className="col col-2">
                  <FormGroup>
                    <Input type="checkbox" />
                    <Label> Zaplaceno na místě </Label>
                  </FormGroup>
                </Col>
              </Row>
              <Row>
                <FormGroup>
                  <Label>Fotky:</Label>
                  <Input type="textarea" />
                </FormGroup>
              </Row>
              {/*<Row>*/}
              <Input type="submit" value="Submit" />
              {/*</Row>*/}
            </Form>
          </Col>
        </Row>
      </div>
    );
  }
}

export default OffenceCreate;
