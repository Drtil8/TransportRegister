import { Component } from "react";
import { Col, Row } from "reactstrap";

export class MyAccount extends Component<object> {
  constructor(props: object) {
    super(props);
  }

  render() {
    return (
      <div>
        <h1>Můj účet</h1>
        <Row>
          <Col>
            email, jmeno, role, ...
          </Col>
        </Row>
        <Row>
          <Col>
            moznost zmenit heslo
          </Col>
        </Row>
      </div>
    );
  }
}
