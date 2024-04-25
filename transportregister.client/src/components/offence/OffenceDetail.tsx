import { Component, FormEvent } from 'react';
import { Col, Form, FormGroup, Input, Label, Row } from 'reactstrap';
//import LocationPicker from '../location/LocationPicker';

interface OffenceDetailProps {
  pid: number;
}

class OffenceDetail extends Component {
  render() {
    return (
      <div className="container-fluid">
        <Row>
          <Col xs="10">
            <Row>
              <Col>
                <h4>Detail přestupku - PID</h4>
              </Col>
            </Row>
            <Row>
              <dl>
                <Row>
                  <dt>Popis:</dt>
                  <dd><textarea readOnly value="todo" className="form-control" /></dd>
                </Row>
                <Row>
                  <Col>
                    <dt>Spáchán</dt>
                    <dd>datum</dd>
                  </Col>
                  <Col>
                    <dt>Místo činu:</dt>
                    <dd>adresa</dd>
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <dt>Typ přestupku:</dt>
                    <dd>typ</dd>
                  </Col>
                  <Col>
                    <dt>Trestné body:</dt>
                    <dd>12 treba</dd>
                  </Col>
                </Row>
                <Row>
                  <h4> Pokuta - pouze pokud ji ma </h4>
                </Row>
                <Row>
                  <Col>
                    <dt>Výše pokuty:</dt>
                    <dd>castka Kc</dd>
                  </Col>
                  <Col>
                    <dt>Zaplaceno</dt>
                    <dd>NE/ANO - datum kdy</dd>
                  </Col>
                </Row>
              </dl>
            </Row>
          </Col>
          <Col>
            panel operací
          </Col>
        </Row>
      </div>
    );
  }
}

export default OffenceDetail;
