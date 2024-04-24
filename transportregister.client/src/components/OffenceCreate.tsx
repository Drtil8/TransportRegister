import { Component } from 'react';
import { Col, Row } from 'reactstrap';
export class OffenceCreate extends Component<object> {
  constructor(props: object) {
    super(props);
  }

  render() {
    return (
      <div>
        <Row>
          <Col>
            <h4>Vytvořit přestupek</h4>
          </Col>
        </Row>
        <Row>
          <Col>
            View/form pro vytvoření přestupku
          </Col>
        </Row>
      </div>
    );
  }
}

export default OffenceCreate;
