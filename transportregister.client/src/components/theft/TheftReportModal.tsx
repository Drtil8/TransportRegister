import React, { FormEvent, useState } from "react";
import { Button, Col, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader, Row } from "reactstrap";

interface TheftReportModalProps {
}

const TheftReportModal: React.FC<TheftReportModalProps> = () => {
  const [modal, setModal] = useState(false);
  const initialFormData = {
    reportTheftVehicleId: 1, // TODO
    reportTheftStolenOn: "",
    reportTheftDescription: "",
    reportTheftAddress: "todo",
    reportTheftPhotos: "todo",
    reportTheftVIN: "fetch z detailu",
    reportTheftSPZ: "fetch z detailu",
    reportTheftBrand: "fetch z detailu",
    reportTheftModel: "fetch z detailu",
    reportTheftOwnerId: 1, // TODO
    reportTheftOwnerFirstName: "fetch z detailu",
    reportTheftOwnerLastName: "fetch z detailu",
    reportTheftOwnerBirthNumber: "fetch z detailu",
    reportTheftOwnerBirthDate: "fetch z detailu",
  }
  const [formData, setFormData] = useState(initialFormData);
  const toggle = () => setModal(!modal);

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      const response = await fetch("/api/Theft/ReportTheft", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          description: formData.reportTheftDescription,
          stolenOn: formData.reportTheftStolenOn,
          //address: formData.reportTheftAddress,
          vehicleId: formData.reportTheftVehicleId,
          reportingPersonId: formData.reportTheftOwnerId,
        }),
      });

      if (!response.ok) {
        console.log("Něco se nepovedlo.");
      }
    }
    catch (error) {
      console.log(error);
    }
    console.log(formData);
    toggle();
  };

  const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = event.target;
    setFormData({ ...formData, [name]: value });
  }

  return (
    <div>
      <Button color="danger" onClick={toggle}>Nahlásit krádež vozidla</Button>
      <Modal isOpen={modal} toggle={toggle}>
        <ModalHeader toggle={toggle}>Nahlášení krádeže vozidla</ModalHeader>
        <Form onSubmit={handleSubmit}>
          <ModalBody>
            <Row>
              <FormGroup>
                <Label>
                  <b>Krádež:</b>
                </Label>
                <Row className="mb-1">
                  <Col>
                    <Label>
                      Popis krádeže:
                    </Label>
                    <Input id="reportTheftDescription" name="reportTheftDescription" type="textarea" value={formData.reportTheftDescription} onChange={handleChange} />
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <Label>Ukradeno dne:</Label>
                    <Input id="reportTheftStolenOn" name="reportTheftStolenOn" type="datetime-local" value={formData.reportTheftStolenOn} onChange={handleChange} />
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <Label>
                      Místo činu:
                    </Label>
                    <Input id="reportTheftAddress" name="reportTheftAddress" type="text" value={formData.reportTheftAddress} onChange={handleChange} />
                  </Col>
                </Row>
                {/*<Row>*/}
                {/*  <Col>*/}
                {/*    <Label>*/}
                {/*      Fotky:*/}
                {/*    </Label>*/}
                {/*    <Input id="reportTheftPhotos" name="reportTheftPhotos" type="textarea" />*/}
                {/*  </Col>*/}
                {/*  </Row>*/}
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label>
                  <b>Vozidlo:</b>
                </Label>
                <Row>
                  <Col>
                    <Label> VIN: </Label>
                    <Input id="reportTheftVIN" name="reportTheftVIN" readOnly type="text" value={formData.reportTheftVIN} onChange={handleChange} />
                  </Col>
                  <Col>
                    <Label> SPZ: </Label>
                    <Input id="reportTheftSPZ" name="reportTheftSPZ" readOnly type="text" value={formData.reportTheftSPZ} onChange={handleChange} />
                  </Col>
                </Row>
                <Row id="reportTheftDetailRow">
                  <Col>
                    <Label> Značka: </Label>
                    <Input id="reportTheftBrand" name="reportTheftBrand" readOnly type="text" value={formData.reportTheftBrand} onChange={handleChange} />
                  </Col>
                  <Col>
                    <Label> Model: </Label>
                    <Input id="reportTheftModel" name="reportTheftModel" readOnly type="text" value={formData.reportTheftModel} onChange={handleChange} />
                  </Col>
                </Row>
                <Row>
                  <Col>
                  </Col>
                  <Col>
                  </Col>
                </Row>
              </FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <Label>
                  <b>Vlastník:</b>
                </Label>
                <Row>
                  <Col>
                    <Label> Jméno: </Label>
                    <Input readOnly id="reportTheftOwnerFirstName" name="reportTheftOwnerFirstName" type="text" value={formData.reportTheftOwnerFirstName} />
                  </Col>
                  <Col>
                    <Label> Příjmení: </Label>
                    <Input readOnly id="reportTheftOwnerLastName" name="reportTheftOwnerLastName" type="text" value={formData.reportTheftOwnerLastName} />
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <Label> Rodné číslo: </Label>
                    <Input readOnly id="reportTheftOwnerBirthNumber" name="reportTheftOwnerBirthNumber" type="text" value={formData.reportTheftOwnerBirthNumber} />
                  </Col>
                  <Col>
                    <Label> Datum narození: </Label>
                    <Input readOnly id="reportTheftOwnerBirthDate" name="reportTheftOwnerBirthDate" type="text" value={formData.reportTheftOwnerBirthDate} />
                  </Col>
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

export default TheftReportModal;
