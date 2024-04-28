import React, { FormEvent, useState } from "react";
import { Button, Col, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader, Row } from "reactstrap";
import IOffenceType from "../interfaces/IOffenceType";
interface OffenceReportVehicleModalProps {
}

const OffenceReportVehicleModal: React.FC<OffenceReportVehicleModalProps> = () => {
  const [modal, setModal] = useState(false);
  const initialFormData = {
    reportVehicleType: 1,
    reportVehicleDescription: "",
    //reportVehicleLocation: "", // TODO
    reportVehicleId: 0, // TODO fetch from backend
    reportVehicleVIN: "fetch from detail",
    reportVehicleSPZ: "fetch from detail",
    reportVehicleBrand: "fetch from detail",
    reportVehicleModel: "fetch from detail",
    reportVehicleFineAmount: 0,
    reportVehiclePenaltyPoints: 0,
    reportVehiclePaid: false,
    reportVehicleOwnerFirstName: "fetch from detail",
    reportVehicleOwnerLastName: "fetch from detail",
    reportVehicleOwnerBirthNumber: "fetch from detail",
    reportVehicleOwnerBirthDate: "fetch from detail",
  }

  const [formData, setFormData] = useState(initialFormData);
  const [offenceTypes, setOffenceTypes] = useState<IOffenceType[]>([]);

  const toggle = async () => {
    // fetch offence types
    if (modal === false) {
      try {
        const response = await fetch('api/Offence/GetOffenceTypes', {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
          },
        });
        const data: IOffenceType[] = await response.json();
        setOffenceTypes(data);
      }
      catch (error) {
        console.error('Error:', error);
      }
    }
    else {
      // TODO -> clear form -> mby only when submit
      setFormData(initialFormData); // clears form
    }
    setModal(!modal)
  }

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault();
    // TODO
  }

  const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value, type } = event.target;


    if (name === "reportVehicleFineAmount" || name === "reportVehiclePenaltyPoints") {
      setFormData({ ...formData, [name]: parseFloat(value) });
    }
    else if (type === "checkbox") {
      setFormData({ ...formData, [name]: (event.target as HTMLInputElement).checked });
    }
    else {
      setFormData({ ...formData, [name]: value });
    }
  }

  return (
    <div>
      <Button color="success" onClick={toggle}> Nahlásit přestupek </Button>
      <Modal isOpen={modal} toggle={toggle} backdrop="static">
        <ModalHeader toggle={toggle}>Nahlášení přestupku vozidla</ModalHeader>
        <Form id="reportVehicleForm" onSubmit={handleSubmit}>
          <ModalBody>
            <Row>
              <FormGroup>
                <Label>
                  <b>Přestupek:</b>
                </Label>
                <Row className="mb-1">
                  <Col>
                    <Label>
                      Typ přestupku:
                    </Label>
                    <Input id="reportVehicleType" name="reportVehicleType" type="select" value={formData.reportVehicleType} onChange={handleChange}>
                      {offenceTypes.map((offenceType) => (
                        <option key={offenceType.id} value={offenceType.id}> {offenceType.name} </option>
                      ))}
                    </Input>
                  </Col>
                </Row>
                <Row className="mb-1">
                  <Col>
                    <Label>
                      Popis přestupku:
                    </Label>
                    <Input id="reportVehicleDescription" name="reportVehicleDescription" type="textarea" value={formData.reportVehicleDescription} onChange={handleChange} />
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <Label>
                      Místo činu:
                    </Label>
                    <Input id="reportVehicle" name="reportVehicle" type="text" />
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <Label>
                      Výše pokuty:
                    </Label>
                    <Input id="reportVehicleFineAmount" name="reportVehicleFineAmount" type="number" step={0.01} min={0} value={formData.reportVehicleFineAmount} onChange={handleChange} />
                  </Col>
                  <Col>
                    <Label>
                      Trestné body:
                    </Label>
                    <Input id="reportVehiclePenaltyPoints" name="reportVehiclePenaltyPoints" type="number" max={12} min={0} value={formData.reportVehiclePenaltyPoints} onChange={handleChange} />
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <FormGroup>
                      <Label>
                        <Input id="reportVehiclePaid" name="reportVehiclePaid" type="checkbox" className="me-2" checked={formData.reportVehiclePaid} onChange={handleChange} />
                        Zaplaceno na místě
                      </Label>
                    </FormGroup>
                  </Col>
                </Row>
                {/*<Row>*/}
                {/*  <Col>*/}
                {/*    <Label>*/}
                {/*      Fotky:*/}
                {/*    </Label>*/}
                {/*    <Input id="reportVehiclePhotos" name="reportVehiclePhotos" type="textarea" />*/}
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
                    <Input id="reportVehicleVIN" name="reportVehicleVIN" readOnly type="text" value={formData.reportVehicleVIN} onChange={handleChange} />
                  </Col>
                  <Col>
                    <Label> SPZ: </Label>
                    <Input id="reportVehicleSPZ" name="reportVehicleSPZ" readOnly type="text" value={formData.reportVehicleSPZ} onChange={handleChange} />
                  </Col>
                </Row>
                <Row id="reportVehicleDetailRow">
                  <Col>
                    <Label> Značka: </Label>
                    <Input id="reportVehicleBrand" name="reportVehicleBrand" readOnly type="text" value={formData.reportVehicleBrand} onChange={handleChange} />
                  </Col>
                  <Col>
                    <Label> Model: </Label>
                    <Input id="reportVehicleModel" name="reportVehicleModel" readOnly type="text" value={formData.reportVehicleModel} onChange={handleChange} />
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
                    <Input readOnly id="reportVehicleOwnerFirstName" name="reportVehicleOwnerFirstName" type="text" value={formData.reportVehicleOwnerFirstName} />
                  </Col>
                  <Col>
                    <Label> Příjmení: </Label>
                    <Input readOnly id="reportVehicleOwnerLastName" name="reportVehicleOwnerLastName" type="text" value={formData.reportVehicleOwnerLastName} />
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <Label> Rodné číslo: </Label>
                    <Input readOnly id="reportVehicleOwnerBirthNumber" name="reportVehicleOwnerBirthNumber" type="text" value={formData.reportVehicleOwnerBirthNumber} />
                  </Col>
                  <Col>
                    <Label> Datum narození: </Label>
                    <Input readOnly id="reportVehicleOwnerBirthDate" name="reportVehicleOwnerBirthDate" type="text" value={formData.reportVehicleOwnerBirthDate} />
                  </Col>
                </Row>
              </FormGroup>
            </Row>
            <Row>
              <p id="reportVehicleErrorMsg" className="text-danger hidden"></p>
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
}

export default OffenceReportVehicleModal;
