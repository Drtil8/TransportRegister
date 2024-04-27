import React, { FormEvent, useState } from "react";
import { Button, Col, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader, Row } from "reactstrap";
import IOffenceType from "../interfaces/IOffenceType";

interface OffenceReportDriverModalProps {
  //visible: boolean;
  //  onClose: () => void;
}

const OffenceReportDriverModal: React.FC<OffenceReportDriverModalProps> = () => {
  const [modal, setModal] = useState(false);
  const [formData, setFormData] = useState({
    reportDriverFirstName: "default value bude fetchnuta z detailu",
    reportDriverLastName: "default value bude fetchnuta z detailu",
    reportDriverBirthNumber: "default value bude fetchnuta z detailu",
    reportDriverBirthDate: "default value bude fetchnuta z detailu",
    reportDriverId: 1, // TODO fetch from detail
    reportDriverType: 1,
    reportDriverDescription: "",
    //reportDriverLocation: "", // TODO
    reportDriverVehicleId: 0, // TODO fetch from backend
    reportDriverVehicleVIN: "",
    reportDriverVehicleSPZ: "",
    reportDriverVehicleBrand: "",
    reportDriverVehicleModel: "",
    reportDriverFineAmount: 0,
    reportDriverPenaltyPoints: 0,
    reportDriverPaid: false,

  } as any);
  const [offenceTypes, setOffenceTypes] = useState<IOffenceType[]>([]); // TODO fetch from backend]

  const toggle = async () => {
    // fetch offence types
    try {
      const response = await fetch('api/Offence/GetOffenceTypes', {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });
      const data: IOffenceType[] = await response.json();
      setOffenceTypes(data);
      setModal(!modal)
    }
    catch (error) {
      console.error('Error:', error);
    }
  }

    const handleSubmit = async (event: FormEvent) => {
      event.preventDefault();
      console.log(formData);

      //try {
      //  const response = await fetch('api/Offence/ReportOffence', {
      //    method: 'POST',
      //    headers: {
      //      'Content-Type': 'application/json',
      //    },
      //    body: JSON.stringify({
      //      description: formData.reportDriverDescription,
      //      personId: formData.reportDriverId,
      //      vehicleId: formData.reportDriverVehicleId,
      //      fineAmount: formData.reportDriverFineAmount,
      //      penaltyPoints: formData.reportDriverPenaltyPoints,
      //      finePaid: formData.reportDriverPaid,
      //      //type: formData.reportDriverType,
      //      //address: formData.reportDriverLocation,

      //    }),
      //  });

      //  if (!response.ok) {
      //    const errP = document.getElementById('reportDriverErrorMsg');
      //    if (errP) {
      //      const errMsg = await response.text();
      //      errP.innerText = errMsg;
      //      errP.classList.remove('hidden');
      //    }
      //    throw new Error('Network response was not ok');
      //  }
      //  else {
      //    toggle();
      //  }
      //}
      //catch (error) {
      //  console.error('Error:', error);
      //}
    }

    const fetchVehicleData = async () => {
      console.log("fetching vehicle data");
      const errMsg = document.getElementById('reportDriverErrorMsg');
      errMsg!.classList.add('hidden');
      if (formData.reportDriverVehicleVIN !== "" || formData.reportDriverVehicleSPZ !== "") {
        try {
          const response = await fetch('api/Offence/GetVehicleForReport', {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
            },
            body: JSON.stringify({
              vin: formData.reportDriverVehicleVIN,
              spz: formData.reportDriverVehicleSPZ,
            }),
          });

          if (!response.ok) {
            throw new Error(await response.text());
          }
          else {
            const data = await response.json();
            setFormData({
              ...formData,
              reportDriverVehicleId: data.id,
              reportDriverVehicleVIN: data.vin,
              //reportDriverVehicleSPZ: data.spz,
              reportDriverVehicleBrand: data.manufacturer,
              reportDriverVehicleModel: data.model,
            });
            document.getElementById('reportDriverVehicleDetailRow')!.classList.remove('hidden');
          }
        }
        catch (error: any) {
          // set values of inputs to default
          setFormData({
            ...formData,
            reportDriverVehicleId: 0,
            //reportDriverVehicleVIN: "",
            //reportDriverVehicleSPZ: "",
            reportDriverVehicleBrand: "",
            reportDriverVehicleModel: "",
          });
          document.getElementById('reportDriverVehicleDetailRow')!.classList.add('hidden');

          errMsg!.classList.remove('hidden');
          errMsg!.innerHTML = error.message;
        }
      }
      else {
        setFormData({
          ...formData,
          reportDriverVehicleId: 0,
          reportDriverVehicleVIN: "",
          reportDriverVehicleSPZ: "",
          reportDriverVehicleBrand: "",
          reportDriverVehicleModel: "",
        });
        document.getElementById('reportDriverVehicleDetailRow')!.classList.add('hidden');
      }
    }

    const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
      const { name, value, type } = event.target;


      if (name === "reportDriverFineAmount" || name === "reportDriverPenaltyPoints") {
        setFormData({ ...formData, [name]: parseFloat(value) });
      }
      else if (type === "checkbox") {
        setFormData({ ...formData, [name]: (event.target as HTMLInputElement).checked });
      }
      else {
        setFormData({ ...formData, [name]: value });
      }

      //if (name == "reportDriverVehicleVIN" || name == "reportDriverVehicleSPZ") {
      //  // todo fetch vehicle data
      //  fetchVehicleData();
      //}
    }

    return (
      <div>
        <Button color="success" onClick={toggle}>
          Nahlásit Přestupek
        </Button>
        <Modal isOpen={modal} toggle={toggle}>
          <ModalHeader toggle={toggle}>Nahlášení přestupku řidiče</ModalHeader>
          <Form id="reportDriverForm" onSubmit={handleSubmit}>
            <ModalBody>
              <Row>
                <FormGroup>
                  <Label>
                    <b>Řidič:</b>
                  </Label>
                  <Row>
                    <Col>
                      <Label> Jméno: </Label>
                      <Input readOnly id="reportDriverFirstName" name="reportDriverFirstName" type="text" value={formData.reportDriverFirstName} />
                    </Col>
                    <Col>
                      <Label> Příjmení: </Label>
                      <Input readOnly id="reportDriverLastName" name="reportDriverLastName" type="text" value={formData.reportDriverLastName} />
                    </Col>
                  </Row>
                  <Row>
                    <Col>
                      <Label> Rodné číslo: </Label>
                      <Input readOnly id="reportDriverBirthNumber" name="reportDriverBirthNumber" type="text" value={formData.reportDriverBirthNumber} />
                    </Col>
                    <Col>
                      <Label> Datum narození: </Label>
                      <Input readOnly id="reportDriverBirthDate" name="reportDriverBirthDate" type="text" value={formData.reportDriverBirthDate} />
                    </Col>
                  </Row>
                </FormGroup>
              </Row>
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
                      <Input id="reportDriverType" name="reportDriverType" type="select" value={formData.reportDriverType} onChange={handleChange}>
                        {offenceTypes.map((offenceType) => (
                          <option key={offenceType.id}> {offenceType.name} </option> 
                        ))}
                      </Input>
                    </Col>
                  </Row>
                  <Row className="mb-1">
                    <Col>
                      <Label>
                        Popis přestupku:
                      </Label>
                      <Input id="reportDriverDescription" name="reportDriverDescription" type="textarea" value={formData.reportDriverDescription} onChange={handleChange} />
                    </Col>
                  </Row>
                  <Row>
                    <Col>
                      <Label>
                        Místo činu:
                      </Label>
                      <Input id="reportDriver" name="reportDriver" type="text" />
                    </Col>
                  </Row>
                  <Row>
                    <Col>
                      <Label>
                        Výše pokuty:
                      </Label>
                      <Input id="reportDriverFineAmount" name="reportDriverFineAmount" type="number" step={0.01} min={0} value={formData.reportDriverFineAmount} onChange={handleChange} />
                    </Col>
                    <Col>
                      <Label>
                        Trestné body:
                      </Label>
                      <Input id="reportDriverPenaltyPoints" name="reportDriverPenaltyPoints" type="number" max={12} min={0} value={formData.reportDriverPenaltyPoints} onChange={handleChange} />
                    </Col>
                  </Row>
                  <Row>
                    <Col>
                      <FormGroup>
                        <Label>
                          <Input id="reportDriverPaid" name="reportDriverPaid" type="checkbox" className="me-2" checked={formData.reportDriverPaid} onChange={handleChange} />
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
                  {/*    <Input id="reportDriverPhotos" name="reportDriverPhotos" type="textarea" />*/}
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
                      <Input id="reportDriverVehicleVIN" name="reportDriverVehicleVIN" type="text" value={formData.reportDriverVehicleVIN} onChange={handleChange} onBlur={fetchVehicleData} />
                    </Col>
                    <Col>
                      <Label> SPZ: </Label>
                      <Input id="reportDriverVehicleSPZ" name="reportDriverVehicleSPZ" type="text" value={formData.reportDriverVehicleSPZ} onChange={handleChange} />
                    </Col>
                  </Row>
                  <Row id="reportDriverVehicleDetailRow" className="hidden">
                    <Col>
                      <Label> Značka: </Label>
                      <Input id="reportDriverVehicleBrand" name="reportDriverVehicleBrand" readOnly type="text" value={formData.reportDriverVehicleBrand} onChange={handleChange} />
                    </Col>
                    <Col>
                      <Label> Model: </Label>
                      <Input id="reportDriverVehicleModel" name="reportDriverVehicleModel" readOnly type="text" value={formData.reportDriverVehicleModel} onChange={handleChange} />
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
                <p id="reportDriverErrorMsg" className="text-danger hidden"></p>
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

  export default OffenceReportDriverModal;
