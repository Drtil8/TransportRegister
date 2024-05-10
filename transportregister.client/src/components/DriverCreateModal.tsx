import React, { FormEvent, useState } from "react";
import { Button, Col, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader, Row } from "reactstrap";
import { IPerson } from "./interfaces/IPersonDetail";
//import { useNavigate } from "react-router-dom";

interface DriverCreateModalProps {
  person: IPerson;
}


const DriverCreateModal: React.FC<DriverCreateModalProps> = ({ person }) => {
  const [modal, setModal] = useState(false);
  //const navigate = useNavigate();
  
  const [formData, setFormData] = useState<{ licensesStrings: string[], driversLicenseNumber: string }>({ 
    licensesStrings: [], 
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

    if (formData.licensesStrings.length == 0) {
      alert("Vyberte alesoň jedno oprávnění");
      return; // Return early if the format is invalid
    }

    try {
      //const urlString: string = "/api/Persons/" + person.personId + "/SetToDriver?license=" + formData.driversLicenseNumber;
      //const urlString: string = "/api/Persons/" + person.personId + "/SetToDriver";
      const urlString: string = "/api/Persons/" + person.personId + "/SetToDriver";

      const response = await fetch(urlString, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          driversLicenseNumber: formData.driversLicenseNumber,
          licenses: formData.licensesStrings,
        }),
      });

      if (!response.ok) {
        console.error("Něco se nepovedlo.");
      }
      else {
        console.log('ok');
      }
    }
    catch (error) {
      console.error(error);
    }

    const params = formData.licensesStrings;
    try {
      const urlstring: string = '/api/Persons/' + person.personId + '/AddDriversLicense';
      const response = await fetch(urlstring, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(params)
      });

      if (response.ok) {
        console.log("Driver licences ok");
        toggle();
        window.location.reload();
      }
      else {
        console.error("Driver licences failed");
      }
    }
    catch (error) {
      console.error('Driver licences failed: ' + error);
    }
    console.log("finished")
  };


  const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = event.target;
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
      <Button color="primary" onClick={toggle}>Vytvořit řidičský průkaz</Button>
      <Modal isOpen={modal} toggle={toggle} backdrop="static" id="DriverCreateModal">
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
