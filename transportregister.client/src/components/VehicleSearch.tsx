import { Component } from 'react';
import { Col, Row } from 'reactstrap';
export class VehicleSearch extends Component<object> {
  constructor(props: object) {
    super(props);
  }

  render() {
    return (
      <div>
        <Row>
          <Col>
            <h4>Vyhledat vozidlo</h4>
          </Col>
          <Col className="d-flex justify-content-end">
            {/*<CreateProjectModal />*/}
          </Col>
        </Row>
        {/*<ProjectsList fetchUrl={'/api/RiskProject/UserRiskProjects'} />*/}
        <div>
          inputy pro vyhledavani
        </div>
      </div>
    );
  }
}

export default VehicleSearch;
