﻿import { Component } from 'react';
import { Col, Row } from 'reactstrap';
import OffenceDatatable from './OffenceDatatable';
import IDtFetchData from '../interfaces/datatables/IDtFetchData';
export class OffencePending extends Component<object> {
  constructor(props: object) {
    super(props);
  }
  fetchDataRef: React.MutableRefObject<IDtFetchData | null> = { current: null };

  render() {
    return (
      <div>
        <Row className="mb-3">
          <Col>
            <h4>Rozpracované přestupky</h4>
          </Col>
        </Row>
        <Row>
          <OffenceDatatable fetchUrl="/api/Offence/Unresolved" fetchDataRef={this.fetchDataRef}></OffenceDatatable>
        </Row>
      </div>
    );
  }
}

export default OffencePending;
