import { Component } from 'react';
import { Col, Row } from 'reactstrap';
export class TheftCreate extends Component<object> {
  constructor(props: object) {
    super(props);
  }

  render() {
    return (
      <div>
        <Row>
          <Col>
            <h4>Zaevidovat krádež vozidla</h4>
          </Col>
        </Row>
        <Row>
          <Col>
            Tabulka kradených a select   nebo  vyhledávání přes input?
          </Col>
        </Row>
      </div>
    );
  }
}

export default TheftCreate;
