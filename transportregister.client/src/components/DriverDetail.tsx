﻿
import { ChangeEvent, Component } from 'react';
import { TabContent, TabPane, Nav, NavItem, NavLink, Row, Col, FormGroup, Input, Label, Form, Button } from 'reactstrap';
import { IPerson, IDriver, IOwner } from './interfaces/IPersonDetail';
import IDriverFormState from './interfaces/IDriverForm';

// TODO fetch the actual driver
interface DriverDetailState {
  activeTab: string;
  personDetail: IPerson | null;
  form: IDriverFormState;
}

export class DriverDetail extends Component<object, DriverDetailState> {
  constructor(props: object) {
    super(props);
    this.state = {
      activeTab: 'detail',
      personDetail: null,
      form: {
        firstName: 'person.firstName',
        lastName: 'person.LastName',
        birthNumber1: 0,
        birthNumber2: 0,
        sexMale: true,
        dateOfBirth: "TODO DATE",
        //address: {
        //  street: '',
        //  city: '',
        //  state: '',
        //  country: '',
        //  houseNumber: '',
        //  postalCode: ''
        //},
        image: '',
        //officialId: '',
        driversLicenseNumber: '0',
        badPoints: 4,
        hasSuspendedLicense: false,
        lastCrimeCommited: '',
        drivingSuspendedUntil: '',
        licenses: [],
        disableInput: true,
      }
    };
  }

  toggleTab = (tab: string) => {
    if (this.state.activeTab !== tab) {
      this.setState({ activeTab: tab });
    }
  }

  componentDidMount() {
    this.populatePersonData();
  }

  async populatePersonData() {
    const urlSplitted = window.location.pathname.split('/');
    const id = urlSplitted[2];

    try {
      const response = await fetch(`/api/Persons/${id}`);
      if (!response.ok) {
        throw new Error(`Failed to load PersonById.`);
      }
      const person = await response.json();
      let parsedPerson: IPerson;
      switch (person.personType) {
        case 'Driver':
          parsedPerson = person as IDriver;
          break;
        case 'Owner':
          parsedPerson = person as IOwner;
          break;
        default:
          throw new Error(`Unknown person type: ${person.personType}`);
      }
      console.log(parsedPerson);    // todo delete
      this.setState({ personDetail: parsedPerson });
    }
    catch (error) {
      console.error('Error fetching person data:', error);
    }
  }

  handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    //this.setState({ [name]: value } as unknown as Pick<DriverFormState, keyof DriverFormState>);
    this.setState(prevState => ({
      form: {
        ...prevState.form,
        [name]: value
      }
    }));
    console.log(name, ' ', value);
  }

  handleChangeCheckbox = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, checked } = e.target;
    this.setState(prevState => ({
      form: {
        ...prevState.form,
        [name]: checked // Update the value associated with the checkbox name
      }
    }));
    console.log(name, ' ', checked);
  }

  switchEditState = () => {
    this.setState(prevState => ({
      form: {
        ...prevState.form,
        disableInput: !prevState.form.disableInput // Toggle the value of edit
      }
    }));
    console.log('switching state', this.state.form.disableInput);
  }

  render() {
    const { activeTab, personDetail } = this.state;
    const form = this.state.form;

    let infoButtons =
      <div>
        <Button onClick={this.switchEditState} color="primary">Zrušit</Button>
        <Button color="primary">Potvrdit</Button> 
        {/*TODO Put*/}
      </div>
    if (this.state.form.disableInput) {
      infoButtons = <Button onClick={this.switchEditState} color="primary">Editovat</Button>;
    }

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
                <NavLink active={activeTab === 'license'} onClick={() => this.toggleTab('license')}> Řidický průkaz </NavLink>
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
                    <br></br>
                    <h5>Osobní informace</h5>
                    <p>{`${personDetail?.firstName} ${personDetail?.lastName}`}</p>
                    <Form>
                      <FormGroup floating>
                        <Input id="firstName" name="firstName" placeholder="firstName" type="text" value={form.firstName} onChange={this.handleChange} required disabled={form.disableInput} />
                        <Label for="firstName">Křestní jméno</Label>
                      </FormGroup>

                      <FormGroup floating>
                        <Input id="firstName" name="lastName" placeholder="lastName" type="text" value={form.lastName} onChange={this.handleChange} required disabled={form.disableInput} />
                        <Label for="lastName">Příjmení:</Label>
                      </FormGroup>

                      <FormGroup>
                        <Label for="birthNumber">Rodné číslo:</Label>
                        <div className="birthNumberInput">
                          <Input type="number" id="birthNumber1" name="birthNumber1" maxLength={6} value={form.birthNumber1} onChange={this.handleChange} required disabled={form.disableInput} />
                          <h4>/</h4>
                          <Input type="number" id="birthNumber2" name="birthNumber2" min={0} max={99999} value={form.birthNumber2} onChange={this.handleChange} required disabled={form.disableInput} />
                        </div>
                      </FormGroup>

                      <FormGroup check>
                        <Label check>
                          <Input type="checkbox" id="sexMale" name="sexMale" value="true" checked={form.sexMale} onChange={this.handleChangeCheckbox} disabled={form.disableInput} />
                          Muž
                        </Label>
                      </FormGroup>
                      {/*TODO adresa*/}
                      {/*TODO Image*/}

                      {/*TODO License number format check*/}
                      <h1></h1>
                      <h5>TODO</h5>
                      <p>Fotka, adresa bydlení, oprávnění</p>


                      {infoButtons}
                    </Form>
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
              <TabPane tabId="license">
                <Row>
                  <Col>
                    <br></br>
                    <Form>
                      <FormGroup floating>
                        <Input id="driversLicenseNumber" name="driversLicenseNumber" placeholder="driversLicenseNumber" type="text" maxLength={8} value={form.driversLicenseNumber} onChange={this.handleChange} required disabled={form.disableInput} />
                        <Label for="driversLicenseNumber">Číslo řidičského průkazu</Label>
                      </FormGroup>

                      <p>Oprávněn řídit:</p>
                      <FormGroup check inline>
                        <Input type="checkbox" id="AM" name="AM" value="false" disabled={form.disableInput} />
                        <Label check>AM</Label>
                      </FormGroup>
                      <FormGroup check inline>
                        <Input type="checkbox" id="A1" name="A1" value="false" disabled={form.disableInput} />
                        <Label check>A1</Label>
                      </FormGroup>
                      <FormGroup check inline>
                        <Input type="checkbox" id="A2" name="A2" value="false" disabled={form.disableInput} />
                        <Label check>A2</Label>
                      </FormGroup>
                      <FormGroup check inline>
                        <Input type="checkbox" id="A" name="A" value="false" disabled={form.disableInput} />
                        <Label check>A</Label>
                      </FormGroup>
                      <br></br>
                      <FormGroup check inline>
                        <Input type="checkbox" id="B1" name="B1" value="false" disabled={form.disableInput} />
                        <Label check>B1</Label>
                      </FormGroup>
                      <FormGroup check inline>
                        <Input type="checkbox" id="B" name="B" value="false" disabled={form.disableInput} />
                        <Label check>B</Label>
                      </FormGroup >

                      <h1></h1>
                      <h5>TODO</h5>
                      <p>Fotka, adresa bydlení, oprávnění</p>


                      <Button type="submit">Register</Button>
                    </Form>
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
