import { Component } from 'react';
import { Col, Row } from 'reactstrap';
import IDtFetchData from './interfaces/datatables/IDtFetchData';
import DriverDatatable from './DriverDatatable';

export class DriverList extends Component<object> {
  constructor(props: object) {
    super(props);
  }
  fetchDataRef: React.MutableRefObject<IDtFetchData | null> = { current: null };

  render() {
    return (
      <div>
        <Row>
          <Col>
            <h4>Všichni řidiči</h4>
          </Col>
        </Row>
        <Row>
          <DriverDatatable fetchDataRef={this.fetchDataRef} autoFetch={true} selectable={false} />
        </Row>
      </div>
    );
  }
}

export default DriverList;
