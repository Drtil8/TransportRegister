import React, { Component } from 'react';
import { Button, Col, Row } from 'reactstrap';
import IDtFetchData from '../interfaces/datatables/IDtFetchData';
import VehicleDatatable from './VehicleDatatable';
import { Link } from 'react-router-dom';

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
            <Button tag={Link} to='/vehicle/create' color="success">
              + Vytvořit vozidlo
            </Button>
          </Col>
        </Row>
        <VehicleDatatable fetchDataRef={this.fetchDataRef} autoFetch={false} />
      </>
    );
  }
}

export default VehicleSearch;
