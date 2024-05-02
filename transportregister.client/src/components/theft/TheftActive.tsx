import { Component } from "react";
import { Col, Row } from 'reactstrap';
import IDtFetchData from '../interfaces/datatables/IDtFetchData';
import TheftDatatable from "./TheftDatatable";

export class TheftActive extends Component<object> {
  constructor(props: object) {
    super(props);
  }
  fetchDataRef: React.MutableRefObject<IDtFetchData | null> = { current: null };

  render() {
    return (
      <div>
        <Row className="mb-3">
          <Col>
            <h4>Aktivní odcizení</h4>
          </Col>
        </Row>
        <Row>
          <TheftDatatable fetchUrl="/api/TheftsActive" fetchDataRef={this.fetchDataRef}></TheftDatatable>
        </Row>
      </div>
    );
  }
}
