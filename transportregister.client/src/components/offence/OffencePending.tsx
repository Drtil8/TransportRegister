import { Component } from 'react';
import { Col, Row } from 'reactstrap';
export class OffencePending extends Component<object> {
  constructor(props: object) {
    super(props);
  }

  render() {
    return (
      <div>
        <Row>
          <Col>
            <h4>-Úředník - nevyřešené přestupky</h4>
          </Col>
        </Row>
        <Row>
          <Col>
            Tabulka nevyřešených přestupků s možností přechodu na detail
          </Col>
        </Row>
      </div>
    );
  }
}

export default OffencePending;
