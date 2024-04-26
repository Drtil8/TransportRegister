import { Component } from 'react';
import { Link } from 'react-router-dom';
import { Button, Col, Row } from 'reactstrap';
export class DriverSearch extends Component<object> {
  constructor(props: object) {
    super(props);
  }

  render() {
    return (
      <div>
        <Row>
          <Col>
            <h4>Vyhledat řidiče</h4>
          </Col>
        </Row>
        {/*<ProjectsList fetchUrl={'/api/RiskProject/UserRiskProjects'} />*/}
        <Row>
          <Col>
            inputy pro vyhledavani
          </Col>
          <Link to="/driver/1">
            <Button color="primary">Example driver detail</Button>
          </Link>
        </Row>
      </div>
    );
  }
}

export default DriverSearch;
