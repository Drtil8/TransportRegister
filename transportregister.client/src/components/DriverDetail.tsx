
import { Component } from 'react';
import { TabContent, TabPane, Nav, NavItem, NavLink, Row, Col } from 'reactstrap';


// TODO fetch the actual driver
interface IDriverDetail {
  id: number;
}
interface DriverDetailState {
  id: number;
  activeTab: string;
}
export class DriverDetail extends Component<IDriverDetail, DriverDetailState> {
  constructor(props: IDriverDetail) {
    super(props);
    this.state = {
      id: props.id,
      activeTab: 'detail',
    };
  }


  toggleTab = (tab: string) => {
    if (this.state.activeTab !== tab) {
      this.setState({ activeTab: tab });
    }
  }

  render() {
    const { activeTab } = this.state;

    const contents = (
      <div className="container">
        <h1>Example driver</h1>
        <div className="row">
          <div className="col-9">
            <Nav tabs className="flex-row-reverse">
              <NavItem>
                <NavLink active={activeTab === 'vehicels'} onClick={() => this.toggleTab('vehicels')}> ?Vozidla </NavLink>
              </NavItem>
              <NavItem>
                <NavLink active={activeTab === 'risks'} onClick={() => this.toggleTab('risks')}> Řidický průkaz </NavLink>
              </NavItem>
              <NavItem>
                <NavLink active={activeTab === 'offences'} onClick={() => this.toggleTab('offences')}> Přestupky </NavLink>
              </NavItem>
              <NavItem>
                <NavLink active={activeTab === 'detail'} onClick={() => this.toggleTab('detail')}> Osobní informace </NavLink>
              </NavItem>
            </Nav>
            <TabContent activeTab={activeTab}>
              <TabPane tabId="detail">
                <Row>
                  <Col>
                    <h5>Osobní informace</h5>
                  </Col>
                </Row>
              </TabPane>
              <TabPane tabId="offences">
                <Row>
                  <Col>
                    <h5>Přestupky</h5>
                  </Col>
                </Row>
              </TabPane>
              <TabPane tabId="risks">
                <Row>
                  <Col>
                    <h5>Řidický průkaz</h5>
                  </Col>
                </Row>
              </TabPane>
              <TabPane tabId="vehicels">
                <Row>
                  <Col>
                    <h5>Vozidla řidiče (vlastník)</h5>
                  </Col>
                </Row>
              </TabPane>
            </TabContent>
          </div>
        </div>
      </div>);

    return (
      <div>
        {contents}
      </div>
    );
  }
}

export default DriverDetail;


//import { Component } from 'react';
//import { Col, Row } from 'reactstrap';
//export class DriverDetail extends Component<object> {
//  constructor(props: object) {
//    super(props);
//  }

//  render() {
//    return (
//      <div>
//        <Row>
//          <Col>
//            <h4>Detail řidiče</h4>
//          </Col>
//        </Row>
//        <Row>
//          <Col>
//            View/form pro registraci
//          </Col>
//        </Row>
//      </div>
//    );
//  }
//}

//export default DriverDetail;
