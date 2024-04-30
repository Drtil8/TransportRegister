import { Component } from 'react';
import { Col, Row } from 'reactstrap';
import UserCreateModal from './UserCreateModal';
import UserDatatable from './UserDatatable';
export class UserList extends Component<object> {
  fetchDataRef: React.MutableRefObject<IDtFetchData | null> = { current: null };
  constructor(props: object) {
    super(props);
  }

  render() {
    return (
      <>
        <Row>
          <Col>
          <h4>Uživatelé</h4>
          </Col>
          <Col className="d-flex justify-content-end">
          <UserCreateModal />
          </Col>
        </Row>
        <UserDatatable fetchDataRef={this.fetchDataRef} />
      </>
    );
  }
}

export default UserList;
