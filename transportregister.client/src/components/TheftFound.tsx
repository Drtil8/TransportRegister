import { Component } from 'react';
import { Col, Row } from 'reactstrap';
export class TheftFound extends Component<object> {
  constructor(props: object) {
    super(props);
  }

  render() {
    return (
      <div>
        <Row>
          <Col>
            <h4>Nalezení vozidla</h4>
          </Col>
        </Row>
        <Row>
          <Col>
            Tabulka kradených a select nebo vyhledávání? + info o nalezení
          </Col>
        </Row>
      </div>
    );
  }
}

export default TheftFound;
