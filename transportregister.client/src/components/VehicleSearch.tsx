import React, { Component } from 'react';
import { Col, Row } from 'reactstrap';
import IDtFetchData from './interfaces/datatables/IDtFetchData';
import VehicleDatatable from './VehicleDatatable';

export class VehicleSearch extends Component<object> {
  fetchDataRef: React.MutableRefObject<IDtFetchData | null> = { current: null };
  constructor(props: object) {
    super(props);
  }

  render() {
    return (
      <>
        <Row>
          <Col>
            <h4>Vyhledat vozidlo</h4>
          </Col>
          <Col className="d-flex justify-content-end">
            {/*<CreateVehicleModal />*/}
          </Col>
        </Row>
        <VehicleDatatable fetchDataRef={this.fetchDataRef} />
      </>
    );
  }
}

export default VehicleSearch;
