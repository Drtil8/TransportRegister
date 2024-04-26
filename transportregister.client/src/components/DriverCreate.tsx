import { Component } from 'react';
import { Col, Row } from 'reactstrap';
export class DriverCreate extends Component<object> {
  constructor(props: object) {
    super(props);
  }

  render() {
    return (
      <div>
        <Row>
          <Col>
            <h4>Zaregistrovat řidiče</h4>
          </Col>
        </Row>
        <Row>
          <Col>
            View/form pro registraci
          </Col>
        </Row>
      </div>
    );
  }
}

export default DriverCreate;
