import React, { FormEvent, useState } from "react";
import { Button, Col, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader, Row } from "reactstrap";
import IOffenceType from "../interfaces/IOffenceType";
import GoogleMapsAutocomplete from "../GoogleMapsAutocomplete";
import IAddress from "../interfaces/IAddress";
import { IPerson } from "../interfaces/IPersonDetail";
import { formatDate } from "../../common/DateFormatter";

interface OffenceReportDriverModalProps {
  personDetail: IPerson | null;
}

const OffenceReportDriverModal: React.FC<OffenceReportDriverModalProps> = ({ personDetail }) => {
  const [modal, setModal] = useState(false);
  const [address, setAddress] = useState<IAddress | null>(null);
  const initialFormData = {
    reportDriverFirstName: personDetail?.firstName,
    reportDriverLastName: personDetail?.lastName,
    reportDriverBirthNumber: personDetail?.birthNumber,
    reportDriverBirthDate: formatDate(personDetail!.dateOfBirth),
    reportDriverId: 1,
    reportDriverType: 1,
    reportDriverDescription: "",
    reportDriverVehicleId: 0,
    reportDriverVehicleVIN: "",
    reportDriverVehicleSPZ: "",
    reportDriverVehicleBrand: "",
    reportDriverVehicleModel: "",
    reportDriverFineAmount: 0,
    reportDriverPenaltyPoints: 0,
    reportDriverPaid: false,
    reportDriverPhotos: [] as string[],
  }

  const [formData, setFormData] = useState(initialFormData as any);
  const [offenceTypes, setOffenceTypes] = useState<IOffenceType[]>([]);

  const toggle = async () => {
    // fetch offence types
    if (modal === false) {
      try {
        const response = await fetch('/api/Offence/GetOffenceTypes', {
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

    setModal(!modal)
  }

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault();

    let photosBase64: string[] = [];
    for (let i = 0; i < formData.reportDriverPhotos.length; i++) {
      let imageBase64 = formData.reportDriverPhotos[i];
      if (imageBase64) {
        imageBase64 = imageBase64.replace(/^data:image\/[a-z]+;base64,/, "");
        photosBase64.push(imageBase64);
      }
    }

    try {
      const response = await fetch('/api/Offence/ReportOffence', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          description: formData.reportDriverDescription,
          personId: formData.reportDriverId,
          vehicleId: formData.reportDriverVehicleId,
          fineAmount: formData.reportDriverFineAmount,
          penaltyPoints: formData.reportDriverPenaltyPoints,
          finePaid: formData.reportDriverPaid,
          offenceTypeId: formData.reportDriverType,
          photos: photosBase64,
          address: address,
        }),
      });

      if (!response.ok) {
        const errP = document.getElementById('reportDriverErrorMsg');
        if (errP) {
          const errMsg = await response.text();
          errP.innerText = errMsg;
          errP.classList.remove('hidden');
        }
        throw new Error('Network response was not ok');
      }
      else {
        setFormData(initialFormData);
        toggle();
      }
    }
    catch (error) {
      console.error('Error:', error);
    }
  }

  const fetchVehicleData = async (event: any) => {
    const errMsg = document.getElementById('reportDriverErrorMsg');
    const chosenVIN = event.target.name === "reportDriverVehicleVIN";
    errMsg!.classList.add('hidden');
    if ((formData.reportDriverVehicleVIN !== "" && chosenVIN) || (formData.reportDriverVehicleSPZ !== "" && !chosenVIN)) {
      try {
        const response = await fetch('/api/Offence/GetVehicleForReport', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            vin: formData.reportDriverVehicleVIN,
            licensePlate: formData.reportDriverVehicleSPZ,
          }),
        });

        if (!response.ok) {
          throw new Error(await response.text());
        }
        else {
          const data = await response.json();
          setFormData({
            ...formData,
            reportDriverVehicleId: data.vehicleId,
            reportDriverVehicleVIN: data.vin,
            reportDriverVehicleSPZ: data.licensePlate,
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
          reportDriverVehicleVIN: !chosenVIN ? "" : formData.reportDriverVehicleVIN,
          reportDriverVehicleSPZ: chosenVIN ? "" : formData.reportDriverVehicleSPZ,
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

  const handleSpeedChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { value } = event.target;
    const speed = parseInt(value);
    let points = 0;
    let amount = 0;

    if (speed <= 20) {
      amount = 1000;
      points = 2;
    }
    else if (speed > 20 && speed <= 40) {
      amount = 2500;
      points = 3;
    }
    else {
      amount = 5000;
      points = 5;
    }

    setFormData({
      ...formData,
      reportDriverFineAmount: amount,
      reportDriverPenaltyPoints: points,
    });
  }

  const handleAddressChange = (address: IAddress) => {
    setAddress(address);
  }

  const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value, type } = event.target;

    if (name === "reportDriverFineAmount" || name === "reportDriverPenaltyPoints") {
      setFormData({ ...formData, [name]: parseFloat(value) });
    }
    else if (name === "reportDriverPhotos") {
      const files = (event.target as HTMLInputElement).files;
      let imageFiles: string[] = [];
      for (let i = 0; i < files!.length; i++) {
        const file = files![i];
        if (file) {
          const reader = new FileReader();
          reader.onloadend = () => {
            const imageInBase64 = reader.result as string;
            imageFiles.push(imageInBase64);
          };
          reader.readAsDataURL(files![i]);
        }
      }
      if (imageFiles) {
        setFormData({ ...formData, [name]: imageFiles });
      }
    }
    else if (type === "checkbox") {
      setFormData({ ...formData, [name]: (event.target as HTMLInputElement).checked });
    }
    else {
      setFormData({ ...formData, [name]: value });
    }

    if (name === "reportDriverType") {
      const offenceType = offenceTypes.find((offenceType) => offenceType.id === parseInt(value));
      if (offenceType) {
        const speedRow = document.getElementById("reportDriverSpeedRow");
        if (offenceType.id == 2) {
          // show input for speed
          speedRow?.classList.remove("hidden");
        }
        else {
          // hide input for speed
          speedRow?.classList.add("hidden");
          (document.getElementById("reportDriverSpeed") as HTMLInputElement)!.value = "0";
        }
        setFormData({
          ...formData,
          reportDriverType: offenceType.id,
          reportDriverFineAmount: offenceType.fineAmount,
          reportDriverPenaltyPoints: offenceType.penaltyPoints,
        });
      }
    }
  }

  return (
    <div>
      <Button color="success" onClick={toggle}>
        Nahlásit Přestupek
      </Button>
      <Modal isOpen={modal} toggle={toggle} backdrop="static">
        <ModalHeader toggle={toggle}>Nahlášení přestupku řidiče</ModalHeader>
        <Form id="reportDriverForm" onSubmit={handleSubmit}>
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
                    <Input id="reportDriverType" name="reportDriverType" type="select" value={formData.reportDriverType} onChange={handleChange}>
                      {offenceTypes.map((offenceType) => (
                        <option key={offenceType.id} value={offenceType.id}> {offenceType.name} </option>
                      ))}
                    </Input>
                  </Col>
                </Row>
                <Row id="reportDriverSpeedRow" className="hidden">
                  <Col>
                    <Label>
                      Překročeno o:
                    </Label>
                    <Input id="reportDriverSpeed" name="reportDriverSpeed" type="number" step={1} min={0} onChange={handleSpeedChange} />
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
                <Row>
                  <Col>
                    <Label>
                      Místo činu:
                    </Label>
                    <GoogleMapsAutocomplete onInputChange={handleAddressChange} hideFields="hidden"></GoogleMapsAutocomplete>
                  </Col>
                </Row>
                <Row className="mt-2">
                  <Col>
                    <Label for="reportDriverPhotos">
                      Fotky z místa činu:
                    </Label>
                    <Input
                      id="reportDriverPhotos"
                      name="reportDriverPhotos"
                      type="file"
                      accept="image/*"
                      onChange={handleChange}
                      multiple />
                  </Col>
                </Row>
              </FormGroup>
            </Row>
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
                  <b>Vozidlo:</b>
                </Label>
                <Row>
                  <Col>
                    <Label> VIN: </Label>
                    <Input id="reportDriverVehicleVIN" name="reportDriverVehicleVIN" type="text" value={formData.reportDriverVehicleVIN} onChange={handleChange} onBlur={fetchVehicleData} />
                  </Col>
                  <Col>
                    <Label> SPZ: </Label>
                    <Input id="reportDriverVehicleSPZ" name="reportDriverVehicleSPZ" type="text" value={formData.reportDriverVehicleSPZ} onChange={handleChange} onBlur={fetchVehicleData} />
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
