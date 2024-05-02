
import React, { ChangeEvent, Component, ContextType } from 'react';
import { TabContent, TabPane, Nav, NavItem, NavLink, Row, Col, FormGroup, Input, Label, Form, Button, Table } from 'reactstrap';
import { IPerson, IDriver, IOwner } from './interfaces/IPersonDetail';
import IDriverFormState from './interfaces/IDriverForm';
import IVehicleListItem from './interfaces/IVehicleListItem';
import { Link } from 'react-router-dom';
import DetailIcon from '@mui/icons-material/VisibilityOutlined';
import { formatDate } from '../common/DateFormatter';
import IOffenceListSimple from './interfaces/IOffenceListSimple';
import DriverCreateModal from './DriverCreateModal';
import OffenceReportDriverModal from './offence/OffenceReportDriverModal';
import AuthContext from '../auth/AuthContext';

interface DriverDetailState {
  activeTab: string;
  personDetail: IPerson | null;
  form: IDriverFormState;
  hadLicenses: string[];
}

export class DriverDetail extends Component<object, DriverDetailState> {
  static contextType = AuthContext;
  declare context: ContextType<typeof AuthContext>;
  constructor(props: object) {
    super(props);
    this.state = {
      activeTab: 'detail',
      personDetail: null,
      hadLicenses: [],
      form: {
        driversLicenseNumber: '0',
        badPoints: 0,
        hasSuspendedLicense: false,
        lastCrimeCommited: '',
        drivingSuspendedUntil: '',
        licensesStrings: [],
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
          let driver: IDriver = person as IDriver;
          let licensesStrings = driver.licenses.map(license => license.vehicleType);
          this.setState(prevState => ({
            form: {
              ...prevState.form,
              driversLicenseNumber: driver.driversLicenseNumber,
              badPoints: driver.badPoints,
              hasSuspendedLicense: driver.hasSuspendedLicense,
              lastCrimeCommited: '',
              drivingSuspendedUntil: driver.drivingSuspendedUntil ? formatDate(driver.drivingSuspendedUntil) : '',
              licensesStrings: licensesStrings,
              disableInput: true,
            },
            hadLicenses: licensesStrings
          }));
          this.setState({ personDetail: driver });
          break;
        case 'Person':
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
    this.setState(prevState => ({
      form: {
        ...prevState.form,
        [name]: value
      }
    }));
  }

  handleChangeCheckbox = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, checked } = e.target;
    this.setState(prevState => ({
      form: {
        ...prevState.form,
        licensesStrings: checked ? [...prevState.form.licensesStrings, name] : prevState.form.licensesStrings.filter(l => l !== name)
      }
    }));
  };

  switchEditState = () => {
    if (!this.state.form.disableInput) { //zruseni
      console.log('zrusit')
      this.setState(prevState => ({
        form: {
          ...prevState.form,
          licensesStrings: prevState.hadLicenses,
        }
      }));
    }
    this.setState(prevState => ({
      form: {
        ...prevState.form,
        disableInput: !prevState.form.disableInput
      }
    }));
    this.setState({activeTab : 'license'});
  }

  putPersonData = async () => {
    this.setState(prevState => ({
      form: {
        ...prevState.form,
        disableInput: !prevState.form.disableInput
      }
    }));
    console.log('Putting Data', this.state.form.licensesStrings);

    const result: string[] = this.state.form.licensesStrings.filter(item => !this.state.hadLicenses.includes(item));
    console.log(result);
    const params = result;
    try {
      const urlstring: string = '/api/Persons/' + this.state.personDetail?.personId + '/AddDriversLicense';
      const response = await fetch(urlstring, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(params)
      });

      if (response.ok) {
        console.log('put ok');
        const appended: string[] = [...this.state.hadLicenses, ...params];
        this.setState({ hadLicenses: appended });
      }
      else {
        console.error("Create vehicle failed");
      }
    }
    catch (error) {
      console.error('Create vehicle failed: ' + error);
    }
  }




  render() {
    const { activeTab } = this.state;
    const form = this.state.form;
    const person = this.state.personDetail;
    const isDriver: boolean = (person != null && person.personType == 'Driver');


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
      </div>
    if (this.state.form.disableInput) {
      infoButtons = <div><Button onClick={this.switchEditState} color="primary">Editovat</Button></div>;
    }


    let vehicles = person?.vehicles;

    let vehiclesTab =
      (vehicles == undefined) ?
        (<p>Nebyly naležena žádná vozidla</p>)
        :
        (
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

    const dateOfBirthString = (person != null) ? formatDate(person!.dateOfBirth) : '';
    const imgsrcString = "data:image/png;base64," + person?.imageBase64;
    const licenseBreakpointList = ['A', 'B', 'C', 'D', 'DE'];

    const contents = (
      <div className="container">
        <Row>
          <Col>
            <h1>{person?.firstName} {person?.lastName}</h1>
          </Col>
          <Col className="rightSide">
            {isDriver ?
              (this.context?.isOfficer ? 
                <OffenceReportDriverModal personDetail={this.state.personDetail}></OffenceReportDriverModal>
                :
                infoButtons
              )
              :
              <DriverCreateModal person={person as IPerson}></DriverCreateModal>
            }
          </Col>
        </Row>
        <div className="row">
          <div className="col">
            <Nav tabs className="flex-row-reverse">
              <NavItem>
                <NavLink active={activeTab === 'vehicle'} onClick={() => this.toggleTab('vehicle')}> Vozidla </NavLink>
              </NavItem>
              {isDriver &&
                (<NavItem>
                  <NavLink active={activeTab === 'license'} onClick={() => this.toggleTab('license')}> Řidičský průkaz </NavLink>
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
                    <Row>
                      <Col sm="6" lg="4">
                        <dt>Křestní jméno</dt>
                        <dd>{person?.firstName}</dd>
                      </Col>
                      <Col sm="6" lg="4">
                        <dt>Příjmení</dt>
                        <dd>{person?.lastName}</dd>
                      </Col>
                      <Col sm="6" lg="4">
                        <dt>Pohlaví</dt>
                        <dd>
                          {person?.sex_Male ?
                            (<p>Muž</p>)
                            :
                            (<p>Žena</p>)}
                        </dd>
                      </Col>
                    </Row>
                    <Row>
                      <Col sm="6" lg="8">
                        <dt>Datum narození</dt>
                        <dd>{dateOfBirthString}</dd>
                      </Col>
                      <Col sm="6" lg="4">
                        <dt>Rodné číslo</dt>
                        <dd>{person?.birthNumber}</dd>
                      </Col>
                    </Row>
                    <h1></h1>
                    <div className="licenceImage">
                      {(person?.imageBase64 != undefined) && (
                        <img src={imgsrcString} alt={`Fotka: ${person?.firstName} ${person?.lastName}`} />
                      )}
                    </div>
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
                      <dt>Číslo řidičského průkazu:</dt>
                      <dd>{form.driversLicenseNumber}</dd>
                      <Form id="licenceForm">
                        <dt>Oprávněn řídit:</dt>
                        {['AM', 'A1', 'A2', 'A', 'B1', 'B', 'C1', 'C', 'D1', 'D', 'BE', 'C1E', 'CE', 'D1E', 'DE', 'T'].map((license) => (
                          <React.Fragment key={license}>
                            <FormGroup check inline key={license}>
                              <Input
                                type="checkbox"
                                id={license}
                                name={license}
                                checked={form.licensesStrings.includes(license)}
                                onChange={this.handleChangeCheckbox}
                                disabled={form.disableInput || (this.state.hadLicenses.includes(license))}
                              />
                              <Label check htmlFor={license}>
                                {license}
                              </Label>
                            </FormGroup>
                            {licenseBreakpointList.includes(license) ? <br></br> : null}
                          </React.Fragment>
                        ))}
                        <br></br>
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
