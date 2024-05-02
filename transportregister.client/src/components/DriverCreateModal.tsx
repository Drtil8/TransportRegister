import React, { FormEvent, useState } from "react";
import { Button, Col, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader, Row } from "reactstrap";
import { IPerson } from "./interfaces/IPersonDetail";

interface DriverCreateModalProps {
  person: IPerson;
}


const DriverCreateModal: React.FC<DriverCreateModalProps> = ({ person }) => {
  const [modal, setModal] = useState(false);
  //const [person, setPerson] = useState(person);
  const initialFormData = {
    licensesStrings: [],
    driversLicenseNumber: "",
  }
  const [formData, setFormData] = useState(initialFormData);
  const toggle = () => setModal(!modal);

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
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
    toggle();
  };


  const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    console.log('change');
    const { name, value } = event.target;
    console.log(name, value);
    setFormData({ ...formData, [name]: value });
  }

  const licenseListAll = ['AM', 'A1', 'A2', 'A', 'B1', 'B', 'C1', 'C', 'D1', 'D', 'BE', 'C1E', 'CE', 'D1E', 'DE', 'T'];
  const licenseBreakpointList = ['A', 'B', 'C', 'D', 'DE'];


  return (
    <div>
      <Button color="primary" onClick={toggle}>Vytvořit řidičský průkaz</Button>
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
                <Row >
                  <dt>Oprávněn řídit:</dt>
                  {licenseListAll.map((license) => (
                    <React.Fragment key={license}>
                      <FormGroup check inline key={license}>
                        <Input
                          type="checkbox"
                          id={license}
                          name={license}
                        />
                        <Label check htmlFor={license}>
                          {license}
                        </Label>
                      </FormGroup>
                      {licenseBreakpointList.includes(license) ? <br></br> : null}
                    </React.Fragment>
                  ))}
                </Row>
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
