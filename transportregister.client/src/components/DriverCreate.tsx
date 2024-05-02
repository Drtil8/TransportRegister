import { ChangeEvent, Component, FormEvent } from 'react';
import { Button, Col, Form, FormGroup, Input, Label, Row } from 'reactstrap';
import IDriverFormState from './interfaces/IDriverForm';


interface IDriverCreateState {
  form: IDriverFormState
}

export class DriverCreate extends Component<object, IDriverCreateState> {

  constructor(props: object) {
    super(props);
    this.state = {
      form: {
        firstName: '',
        lastName: '',
        birthNumber1: 0,
        birthNumber2: 0,
        sex_Male: true,
        dateOfBirth: '',
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
        driversLicenseNumber: '',
        badPoints: 0,
        hasSuspendedLicense: false,
        lastCrimeCommited: '',
        drivingSuspendedUntil: '',
        licenses: [],

        disableInput: false
      }
    };
  }

  handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    console.log("//TODO FORM SUBMIT")
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

  render() {

    let form = this.state.form;
    return (
      <div>
        <Row>
          <Col>
            <h4>Zaregistrovat řidiče</h4>
          </Col>
        </Row>
        <Row>
          <Col>
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
                  <Input type="number" id="birthNumber1" name="birthNumber1" maxLength={6} value={form.birthNumber1} onChange={this.handleChange} disabled={form.disableInput} />
                  <h4>/</h4>
                  <Input type="number" id="birthNumber2" name="birthNumber2" min={0} max={99999} value={form.birthNumber2} onChange={this.handleChange} />
                </div>
              </FormGroup>

              <FormGroup check>
                <Label check>
                  <Input type="checkbox" id="sexMale" name="sexMale" value="true" checked={form.sex_Male} onChange={this.handleChangeCheckbox} disabled={form.disableInput} />
                  Muž
                </Label>
              </FormGroup>
              {/*TODO adresa*/}
              {/*TODO Image*/}

              {/*TODO License number format check*/}
              <FormGroup floating>
                <Input id="driversLicenseNumber" name="driversLicenseNumber" placeholder="driversLicenseNumber" type="text" maxLength={8} value={form.driversLicenseNumber} onChange={this.handleChange} required disabled={form.disableInput} />
                <Label for="driversLicenseNumber">Číslo řidičského průkazu</Label>
              </FormGroup>

              <p>Oprávněn řídit:</p>
              <FormGroup check inline>
                <Input type="checkbox" id="AM" name="AM" value="false" />
                <Label check>AM</Label>
              </FormGroup>
              <FormGroup check inline>
                <Input type="checkbox" id="A1" name="A1" value="false" />
                <Label check>A1</Label>
              </FormGroup>
              <FormGroup check inline>
                <Input type="checkbox" id="A2" name="A2" value="false" />
                <Label check>A2</Label>
              </FormGroup>
              <FormGroup check inline>
                <Input type="checkbox" id="A" name="A" value="false" />
                <Label check>A</Label>
              </FormGroup>
              <br></br>
              <FormGroup check inline>
                <Input type="checkbox" id="B1" name="B1" value="false" />
                <Label check>B1</Label>
              </FormGroup>
              <FormGroup check inline>
                <Input type="checkbox" id="B" name="B" value="false" />
                <Label check>B</Label>
              </FormGroup >

              <h1></h1>
              <h5>TODO</h5>
              <p>Fotka, adresa bydlení, oprávnění</p>


              <Button type="submit">Register</Button>
            </Form>
          </Col>
        </Row>
      </div>
    );
  }
}

export default DriverCreate;
