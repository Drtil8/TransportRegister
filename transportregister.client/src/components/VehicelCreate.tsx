import { Component } from 'react';
import { Col, Row } from 'reactstrap';
export class VehicleCreate extends Component<object> {
  constructor(props: object) {
    super(props);
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
          <Col>
            View/form pro vozidla
          </Col>
        </Row>
      </div>
    );
  }
}

export default VehicleCreate;
