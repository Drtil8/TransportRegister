import { Component, ContextType } from 'react';
import { Link, Navigate } from 'react-router-dom';
import AuthContext from '../auth/AuthContext';
import { Col, Row } from 'reactstrap';

export class MainPage extends Component<object> {
  static contextType = AuthContext;
  declare context: ContextType<typeof AuthContext>;

  constructor(props: object) {
    super(props);
  }

  render() {
    if (this.context?.isAdmin) {
      return <Navigate to="/users" replace />;
    }

    return (
      <>
        <h3> Zvolte operaci: </h3>
        <Row className="mainRow">
          <Col>
            <Row>
              <Col className="mainCol-right">
                <Link to="/vehicle/search">
                  <div className="mainCard">
                    VOZIDLA
                  </div>
                </Link>
              </Col>
              <Col className="mainCol-left">
                <Link to="/driverSearch">
                  <div className="mainCard">
                    ŘIDIČI
                  </div>
                </Link>
              </Col>
            </Row>
            <Row>
              <Col className="mainCol-right">
                <Link to="/offencePending">
                  <div className="mainCard">
                    PŘESTUPKY
                  </div>
                </Link>
              </Col>
              <Col className="mainCol-left">
                <Link to="/theftsActive">
                  <div className="mainCard">
                    KRÁDEŽE
                  </div>
                </Link>
              </Col>
            </Row>
          </Col>
        </Row>
      </>
    );
  }
}
