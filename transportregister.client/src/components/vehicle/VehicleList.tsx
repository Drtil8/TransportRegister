import React, { Component, ContextType } from 'react';
import { Button, Col, Row } from 'reactstrap';
import IDtFetchData from '../interfaces/datatables/IDtFetchData';
import VehicleDatatable from './VehicleDatatable';
import { Link } from 'react-router-dom';
import AuthContext from '../../auth/AuthContext';

export class VehicleList extends Component<object> {
  fetchDataRef: React.MutableRefObject<IDtFetchData | null> = { current: null };
  static contextType = AuthContext;
  declare context: ContextType<typeof AuthContext>;
  constructor(props: object) {
    super(props);
  }

  render() {
    return (
      <>
        <Row>
          <Col>
            <h4>Vozidla</h4>
          </Col>
          {this.context?.isOfficial && (
            <Col className="d-flex justify-content-end">
              <Button tag={Link} to='/vehicle/create' color="success">
                + Vytvořit vozidlo
              </Button>
            </Col>
          )}
        </Row>
        <VehicleDatatable fetchDataRef={this.fetchDataRef} autoFetch={true} />
      </>
    );
  }
}

export default VehicleList;
