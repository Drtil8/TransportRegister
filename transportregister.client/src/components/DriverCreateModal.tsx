import React, { FormEvent, useState } from "react";
import { Button, Col, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader, Row } from "reactstrap";

interface DriverCreateModalProps {
  person: IPerson;
}


const DriverCreateModal: React.FC<DriverCreateModalProps> = ({ person }) => {
  const [modal, setModal] = useState(false);
  //const [person, setPerson] = useState(person);
  
  const [formData, setFormData] = useState<{ licensesStrings: string[], driversLicenseNumber: string }>({
    licensesStrings: [], // Specify the type as an array of strings
    driversLicenseNumber: "",
  });

  const toggle = () => setModal(!modal);

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const licenseNumberPattern = /^[A-Z]{2}\d{6}$/; // Example format: AB123456
    if (!licenseNumberPattern.test(formData.driversLicenseNumber)) {
      alert("Nevalidní formát. (dvě velká písmena a šest číslic)\n Příklad: AB123456");
      return; // Return early if the format is invalid
    }

    //try {
    //  const response = await fetch("/api/Theft/ReportTheft", {
    //    method: "POST",
    //    headers: {
    //      "Content-Type": "application/json",
    //    },
    //    //body: JSON.stringify({
    //    //  description: formData.reportTheftDescription,
    //    //  stolenOn: formData.reportTheftStolenOn,
    //    //  vehicleId: formData.reportTheftVehicleId,
    //    //  reportingPersonId: formData.reportTheftOwnerId,
    //    //}),
    //  });

    //  if (!response.ok) {
    //    console.log("Něco se nepovedlo.");
    //  }
    //}
    //catch (error) {
    //  console.log(error);
    //}
    //console.log(formData);
    console.log('submioging');
    console.log(person);
    //const driver: IDriver = {

    //}
    //toggle();
  };


  const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = event.target;
    console.log(name, value);
    setFormData({ ...formData, [name]: value });

  
  }
  const handleChangeCheckbox = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, checked } = e.target;
    setFormData(prevState => {
      let updatedLicenses = [...prevState.licensesStrings];
      if (checked) {
        updatedLicenses.push(name);
      } else {
        updatedLicenses = updatedLicenses.filter(license => license !== name);
      }
      return { ...prevState, licensesStrings: updatedLicenses };
    });
  };

  const licenseListAll: string[] = ['AM', 'A1', 'A2', 'A', 'B1', 'B', 'C1', 'C', 'D1', 'D', 'BE', 'C1E', 'CE', 'D1E', 'DE', 'T'];
  const licenseBreakpointList = ['A', 'B', 'C', 'D', 'DE'];


  return (
    <div>
      <Button color="primary" onClick={toggle}>Vytvořit řidický průkaz</Button>
      <Modal isOpen={modal} toggle={toggle} backdrop="static">
        <ModalHeader toggle={toggle}>Nahlášení krádeže vozidla</ModalHeader>
        <Form onSubmit={handleSubmit}>
          <ModalBody>
            <Row>
              <FormGroup>
                <Label>
                  <b>Vozidlo:</b>
                </Label>
                <Row>
                  <Col>
                    <Label> Číslo řidičského průkazu: </Label>
                    <Input id="driversLicenseNumber" name="driversLicenseNumber" type="text" value={formData.driversLicenseNumber} onChange={handleChange} />
                  </Col>
                </Row>
                  <dt>Oprávněn řídit:</dt>
                  {licenseListAll.map((license) => (
                    <React.Fragment key={license}>
                      <span className="licenceSpan">
                      <FormGroup check inline key={license}>
                        <Input
                          type="checkbox"
                          id={license}
                          name={license}
                          checked={formData.licensesStrings.includes(license)}
                          onChange={handleChangeCheckbox}
                        />
                        <Label check htmlFor={license}>
                          {license}
                        </Label>
                      </FormGroup>
                      </span>
                      {licenseBreakpointList.includes(license) ? <br></br> : null}
                    </React.Fragment>
                  ))}
              </FormGroup>
            </Row>
            <Row>
              <p id="reportTheftErrorMsg" className="text-danger hidden"></p>
            </Row>
          </ModalBody>
          <ModalFooter>
            <Button color="primary" type="submit">
              Nahlásit
            </Button>
            <Button color="secondary" onClick={toggle}>
              Zrušit
            </Button>
          </ModalFooter>
        </Form>
      </Modal>
    </div>
  );
};

export default DriverCreateModal;
