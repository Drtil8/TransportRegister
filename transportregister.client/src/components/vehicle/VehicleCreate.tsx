import { Component } from 'react';
import { Col, Row } from 'reactstrap';
import VehicleForm from './VehicleForm';

export class VehicleCreate extends Component<object> {
  constructor(props: object) {
    super(props);
  }

  render() {
    return (
      <>
        <Row>
          <Col>
            <h4>Registrace vozidla</h4>
          </Col>
        </Row>

        <VehicleForm fetchedVehicle={null} />
      </>
    );
  }
}

export default VehicleCreate;
