import { Component, ContextType } from 'react';
import { Col, Row } from 'reactstrap';
import UserCreateModal from './UserCreateModal';
import UserDatatable from './UserDatatable';
import AuthContext from '../../auth/AuthContext';
import { Navigate } from 'react-router-dom';
import IDtFetchData from '../interfaces/datatables/IDtFetchData';

export class UserList extends Component<object> {
  fetchDataRef: React.MutableRefObject<IDtFetchData | null> = { current: null };
  static contextType = AuthContext;
  declare context: ContextType<typeof AuthContext>;
  constructor(props: object) {
    super(props);
  }

  render() {
    if (!this.context?.isAdmin) {
      return <Navigate to="/" replace />;
    }

    return (
      <>
        <Row className="mb-3">
          <Col>
          <h4>Uživatelé</h4>
          </Col>
          <Col className="d-flex justify-content-end">
            <UserCreateModal fetchDataRef={this.fetchDataRef} />
          </Col>
        </Row>
        <UserDatatable fetchDataRef={this.fetchDataRef} />
      </>
    );
  }
}

export default UserList;
