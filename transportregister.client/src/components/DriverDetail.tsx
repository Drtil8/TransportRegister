
import { ChangeEvent, Component } from 'react';
import { TabContent, TabPane, Nav, NavItem, NavLink, Row, Col, FormGroup, Input, Label, Form, Button, Table } from 'reactstrap';
import IDriverDetail from './interfaces/IDriverDetail';
import IPersonDetail from './interfaces/IPersonDetail';
import IDriverFormState from './interfaces/IDriverForm';
import IVehicleListItem from './interfaces/IVehicleListItem';
import { Link } from 'react-router-dom';
import DetailIcon from '@mui/icons-material/VisibilityOutlined';


// TODO fetch the actual driver

interface DriverDetailState {

  activeTab: string;
  driver: IDriverDetail | null;
  form: IDriverFormState;
}
export class DriverDetail extends Component<object, DriverDetailState> {

  per: IPersonDetail = {
    id: 20,
    FirstName: "John",
    LastName: "Doe",
    BirthNumber: "123456/7890",
    Sex_male: true,
    DateOfBirth: new Date("1990-01-01"),
    // Add other properties as needed
  };

  vehiclesList: IVehicleListItem[] = [
    {
      id: 1,
      vin: "ABC123",
      licensePlate: "XYZ123",
      vehicleType: "Car",
      manufacturer: "Toyota",
      model: "Camry",
      color: "Blue",
      manufacturedYear: 2020,
      ownerId: 123,
      ownerFullName: "John Doe",
    },
    {
      id: 2,
      vin: "DEF456",
      licensePlate: "LMN456",
      vehicleType: "Truck",
      manufacturer: "Ford",
      model: "F-150",
      color: "Red",
      manufacturedYear: 2018,
      ownerId: 456,
      ownerFullName: "Jane Smith",
    },
  ];

  driverDetail: IDriverDetail = {
    person: this.per,
    driversLicenseNumber: 'EN123456',
    badPoints: 3,
    hasSuspendedLicense: false,
    lastCrimeCommited: '',
    drivingSuspendedUntil: '',
    licenses: [],
    vehicles: this.vehiclesList,
  };
    constructor(props: object) {
    super(props);
    const person = this.driverDetail.person;
    const driver = this.driverDetail;
    this.state = {
      activeTab: 'detail',
      driver: this.driverDetail,
      form: {
        firstName: person.FirstName,
        lastName: person.LastName,
        birthNumber1: 0,
        birthNumber2: 0,
        sexMale: person.Sex_male,
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
        driversLicenseNumber: driver.driversLicenseNumber,
        badPoints: driver.badPoints,
        hasSuspendedLicense: driver.hasSuspendedLicense,
        lastCrimeCommited: driver.lastCrimeCommited,
        drivingSuspendedUntil: driver.drivingSuspendedUntil,
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

  async populateProjectDetail() {
    //const urlSplitted = window.location.pathname.split('/');
    //const id = urlSplitted[2];

    //const apiUrl = `/api/RiskProject/${id}`;
    //try {
    //  const response = await fetch(apiUrl);
    //  const data: IDriverDetail = await response.json();
    //  //this.setState({ projectDetail: data });
    //} catch (error) {
    //  console.error('Error fetching project detail:', error);
    //}
    /*const data = ;*/
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
    const { activeTab } = this.state;
    const form = this.state.form;
    const vehicles = this.driverDetail.vehicles;

    let infoButtons =
      <div>
        <Button onClick={this.switchEditState} >Zrušit</Button>
        <Button >Potvrdit</Button> 
        {/*TODO Put*/}
      </div>
    if (this.state.form.disableInput) {
      infoButtons = < Button onClick={this.switchEditState} > Editovat</Button>;
    }



    const contents = (
      <div className="container">
        <h1>Example driver</h1>
        <h2>{ }</h2>
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
                    <p>{this.driverDetail.person.FirstName}</p>
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
                    <Table>
                      <thead>
                        <tr>
                          {/*<th>ID</th>*/}
                          <th>VIN</th>
                          <th>SPZ</th>
                          <th>Typ</th>
                          <th>Výrobce</th>
                          <th>Model</th>
                          <th>Barva</th>
                          <th>Rok výroby</th>
                          {/*<th>Owner ID</th>*/}
                          <th>Majitel</th>
                        </tr>
                      </thead>
                      <tbody>
                        {vehicles.map((vehicle: IVehicleListItem) => (
                          <tr key={vehicle.id}>
                            {/*<td>{vehicle.id}</td>*/}
                            <td>{vehicle.vin}</td>
                            <td>{vehicle.licensePlate}</td>
                            <td>{vehicle.vehicleType}</td>
                            <td>{vehicle.manufacturer}</td>
                            <td>{vehicle.model}</td>
                            <td>{vehicle.color}</td>
                            <td>{vehicle.manufacturedYear}</td>
                            {/*<td>{vehicle.ownerId}</td>*/}
                            <td>{vehicle.ownerFullName}</td>
                            <td>
                              <Link to={`/vehicle/${vehicle.id}`}>
                                <DetailIcon />
                              </Link>
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </Table>
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
