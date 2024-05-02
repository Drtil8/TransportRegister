
import { ChangeEvent, Component } from 'react';
import { TabContent, TabPane, Nav, NavItem, NavLink, Row, Col, FormGroup, Input, Label, Form, Button, Table } from 'reactstrap';
import { IPerson, IDriver, IOwner } from './interfaces/IPersonDetail';
import IDriverFormState from './interfaces/IDriverForm';
import IVehicleListItem from './interfaces/IVehicleListItem';
import { Link } from 'react-router-dom';
import DetailIcon from '@mui/icons-material/VisibilityOutlined';
import { formatDate } from '../common/DateFormatter';
import IOffenceListSimple from './interfaces/IOffenceListSimple';

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
        firstName: '',
        lastName: '',
        birthNumber1: 0,
        birthNumber2: 0,
        sex_Male: true,
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
        badPoints: 0,
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
      let parsedPerson: IPerson = person as IPerson;
      this.setState(prevState => ({
        form: {
          ...prevState.form,
          firstName: parsedPerson.firstName,
          lastName: parsedPerson.lastName,
          birthNumber1: 0,
          birthNumber2: 0,
          sex_Male: parsedPerson.sex_Male,
          dateOfBirth: "TODO DATE",
          image: ''
        }
      }));
      switch (person.personType) {
        case 'Driver':
          //parsedPerson = person as IOwner;
          let driver: IDriver = person as IDriver;
          this.setState(prevState => ({
            form: {
              ...prevState.form,
              driversLicenseNumber: driver.driversLicenseNumber,
              badPoints: driver.badPoints,
              hasSuspendedLicense: driver.hasSuspendedLicense,
              lastCrimeCommited: '',
              drivingSuspendedUntil: driver.drivingSuspendedUntil ? formatDate(driver.drivingSuspendedUntil) : '',
              licenses: driver.licenses,
              disableInput: true,
            }
          }));
          this.setState({ personDetail: driver });
          break;
        case 'Owner':
          parsedPerson = person as IOwner;
          this.setState({ personDetail: parsedPerson });
          break;
        default:
          throw new Error(`Unknown person type: ${person.personType}`);
      }
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
    //console.log(name, ' ', value);
  }

  handleChangeCheckbox = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, checked } = e.target;
    this.setState(prevState => ({
      form: {
        ...prevState.form,
        [name]: checked // Update the value associated with the checkbox name
      }
    }));
    //console.log(name, ' ', checked);
  }

  switchEditState = () => {
    this.setState(prevState => ({
      form: {
        ...prevState.form,
        disableInput: !prevState.form.disableInput // Toggle the value of edit
      }
    }));
    //console.log('switching state', this.state.form.disableInput);
  }

  putPersonData= () => {
    this.setState(prevState => ({
      form: {
        ...prevState.form,
        disableInput: !prevState.form.disableInput // Toggle the value of edit
      }
    }));
    //console.log('switching state', this.state.form.disableInput);
  }




  render() {
    const { activeTab, personDetail } = this.state;
    const form = this.state.form;
    const person = this.state.personDetail;
    //console.log("logging person", person);
    const isDriver: boolean = (person != null && person.personType == 'Driver');
    //console.log("is driver", isDriver);
    let driver: IDriver | undefined = undefined;
    if (isDriver)
      driver = person as IDriver;



    const hardoffences: IOffenceListSimple[] = [
      {
        offenceId: 1,
        reportedOn: new Date('2024-04-30'),
        description: 'Speeding',
        penaltyPoints: 3,
        fineAmount: 100
      },
      {
        offenceId: 2,
        reportedOn: new Date('2024-04-25'),
        description: 'Running a red light',
        penaltyPoints: 5,
        fineAmount: 150
      },
      {
        offenceId: 3,
        reportedOn: new Date('2024-04-20'),
        description: 'Illegal parking',
        penaltyPoints: 2,
        fineAmount: 50
      }
    ];


    let infoButtons =
      <div>
        <Button onClick={this.switchEditState} color="danger">Zrušit</Button>
        <Button onClick={this.putPersonData} color="primary">Potvrdit</Button>
        {/*TODO Put*/}
      </div>
    if (this.state.form.disableInput) {
      infoButtons = <Button onClick={this.switchEditState} color="primary">Editovat</Button>;
    }


    let vehicles = person?.vehicles;
    //let vehicles = hardVehicles;
    //console.log("vehicles", person?.imageBase64);
    //vehicles = null;

    let vehiclesTab =
      (vehicles == undefined) ?
        (<p>Nebyly naležena žádná vozidla</p>)
        :
        (
          //<p></p>
          <Table>
            <thead>
              <tr>
                <th>ID</th>
                <th>VIN</th>
                <th>SPZ</th>
                <th>Typ</th>
                <th>Výrobce</th>
                <th>Model</th>
                <th>Barva</th>
                <th>Rok výroby</th>
                <th>Owner ID</th>
                <th>Majitel</th>
                <th>Zobrazit</th>
              </tr>
            </thead>
            <tbody>
              {vehicles!.map((vehicle: IVehicleListItem) => (
                <tr key={vehicle.id}>
                  <td>{vehicle.id}</td>
                  <td>{vehicle.vin}</td>
                  <td>{vehicle.licensePlate}</td>
                  <td>{vehicle.vehicleType}</td>
                  <td>{vehicle.manufacturer}</td>
                  <td>{vehicle.model}</td>
                  <td>{vehicle.color}</td>
                  <td>{vehicle.manufacturedYear}</td>
                  <td>{vehicle.ownerId}</td>
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
        );

    //console.log(person?.sex_Male);

    const imgsrcString = "data:image/png;base64," + person?.imageBase64;
    const contents = (
      <div className="container">
        <h1>{person?.firstName} {person?.lastName}</h1>
        <div className="row">
          <div className="col-9">
            <Nav tabs className="flex-row-reverse">
              <NavItem>
                <NavLink active={activeTab === 'vehicle'} onClick={() => this.toggleTab('vehicle')}> ?Vozidla </NavLink>
              </NavItem>
              {isDriver &&
                (<NavItem>
                  <NavLink active={activeTab === 'license'} onClick={() => this.toggleTab('license')}> Řidický průkaz </NavLink>
                </NavItem>)}
              {isDriver &&
                (<NavItem>
                  <NavLink active={activeTab === 'offences'} onClick={() => this.toggleTab('offences')}> Přestupky </NavLink>
                </NavItem>)}
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
                          <Input type="number" id="birthNumber1" name="birthNumber1" maxLength={6} value={form.birthNumber1} onChange={this.handleChange} required disabled={true} />
                          <h4>/</h4>
                          <Input type="number" id="birthNumber2" name="birthNumber2" min={0} max={99999} value={form.birthNumber2} onChange={this.handleChange} required disabled={true} />
                        </div>
                      </FormGroup>

                      {/*<FormGroup check>*/}
                      {/*  <Label check>*/}
                      {/*    <Input type="checkbox" id="sex_Male" name="sex_Male" value="true" checked={form.sex_Male} onChange={this.handleChangeCheckbox} disabled={form.disableInput} />*/}
                      {/*    Muž*/}
                      {/*  </Label>*/}
                      {/*</FormGroup>*/}
                      {person?.sex_Male ?
                        (<p>Muž</p>)
                        :
                        (<p>Žena</p>)}
                      {/*TODO adresa*/}
                      {/*TODO Image*/}

                      {/*TODO License number format check*/}
                      <h1></h1>
                      <h5>TODO</h5>
                      <p>adresa bydlení</p>
                      <div className="licenceImage">
                        {(person?.imageBase64 != undefined) && (
                          <img src={imgsrcString} alt={`Fotka: ${person?.firstName} ${person?.lastName}`} />
                        )}
                      </div>

                      <div>
                        {infoButtons}
                      </div>
                    </Form>
                  </Col>
                </Row>
              </TabPane>
              {isDriver && (
                <TabPane tabId="offences">
                  <Row>
                    <Col>
                      <br></br>
                      <h5>Přestupky</h5>
                      <Table>
                        <thead>
                          <tr>
                            <th>ID</th>
                            <th>Přestupek</th>
                            <th>Body</th>
                            <th>Pokuta</th>
                            <th>Zobrazit</th>
                          </tr>
                        </thead>
                        <tbody>
                          {hardoffences!.map((offence: IOffenceListSimple) => (
                            <tr key={offence.offenceId}>
                              <td>{offence.offenceId}</td>
                              <td>{offence.description}</td>
                              <td>{offence.penaltyPoints}</td>
                              <td>{offence.fineAmount} Kč</td>
                              <td>
                                <Link to={`/offence/${offence.offenceId}`}>
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
              )}
              {isDriver && (
                <TabPane tabId="license">
                  <Row>
                    <Col>
                      <br></br>
                      <Form id="licenceForm">
                        <FormGroup floating className={driver?.hasSuspendedLicense ? "suspendedLicence" : ""}>
                          {/*<div >*/}
                            <Input id="driversLicenseNumber" name="driversLicenseNumber" placeholder="driversLicenseNumber" type="text" maxLength={8} value={form.driversLicenseNumber} onChange={this.handleChange} required disabled={form.disableInput} />
                            <Label for="driversLicenseNumber">Číslo řidičského průkazu</Label>
                          {/*</div>*/}
                        </FormGroup>

                        <p>Oprávněn řídit:</p>
                        <FormGroup check inline>
                          <Input type="checkbox" id="AM" name="AM" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="AM">AM</Label>
                        </FormGroup>
                        <FormGroup check inline>
                          <Input type="checkbox" id="A1" name="A1" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="A1">A1</Label>
                        </FormGroup>
                        <FormGroup check inline>
                          <Input type="checkbox" id="A2" name="A2" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="A2">A2</Label>
                        </FormGroup>
                        <FormGroup check inline>
                          <Input type="checkbox" id="A" name="A" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="A">A</Label>
                        </FormGroup>
                        <br></br>
                        <FormGroup check inline>
                          <Input type="checkbox" id="B1" name="B1" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="B1">B1</Label>
                        </FormGroup>
                        <FormGroup check inline>
                          <Input type="checkbox" id="B" name="B" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="B">B</Label>
                        </FormGroup>
                        <br></br>
                        <FormGroup check inline>
                          <Input type="checkbox" id="C1" name="C1" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="C1">C1</Label>
                        </FormGroup>
                        <FormGroup check inline>
                          <Input type="checkbox" id="C" name="C" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="C">C</Label>
                        </FormGroup>
                        <br></br>
                        <FormGroup check inline>
                          <Input type="checkbox" id="D1" name="D1" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="D1">D1</Label>
                        </FormGroup>
                        <FormGroup check inline>
                          <Input type="checkbox" id="D" name="D" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="D">D</Label>
                        </FormGroup>
                        <br></br>
                        <FormGroup check inline>
                          <Input type="checkbox" id="BE" name="BE" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="BE">BE</Label>
                        </FormGroup>
                        <FormGroup check inline>
                          <Input type="checkbox" id="C1E" name="C1E" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="C1E">C1E</Label>
                        </FormGroup>
                        <FormGroup check inline>
                          <Input type="checkbox" id="CE" name="CE" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="CE">CE</Label>
                        </FormGroup>
                        <FormGroup check inline>
                          <Input type="checkbox" id="D1E" name="D1E" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="D1E">D1E</Label>
                        </FormGroup>
                        <FormGroup check inline>
                          <Input type="checkbox" id="DE" name="DE" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="DE">DE</Label>
                        </FormGroup>
                        <br></br>
                        <FormGroup check inline>
                          <Input type="checkbox" id="T" name="T" value="false" disabled={form.disableInput} />
                          <Label check htmlFor="T">T</Label>
                        </FormGroup>
                        <div>
                          {infoButtons}
                        </div>
                      </Form>
                    </Col>
                  </Row>
                </TabPane>
              )}
              <TabPane tabId="vehicle">
                <Row>
                  <Col>
                    <br></br>
                    <h5>Vozidla řidiče (vlastník)</h5>
                    {vehiclesTab}

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
