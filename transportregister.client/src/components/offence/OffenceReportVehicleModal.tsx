import React, { FormEvent, useState } from "react";
import { Button, Col, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader, Row } from "reactstrap";
import IOffenceType from "../interfaces/IOffenceType";
import IAddress from "../interfaces/IAddress";
import GoogleMapsAutocomplete from "../GoogleMapsAutocomplete";
import { IVehicleDetail } from "../interfaces/IVehicleDetail";
interface OffenceReportVehicleModalProps {
  vehicleDetail: IVehicleDetail | null;
}

const OffenceReportVehicleModal: React.FC<OffenceReportVehicleModalProps> = ({ vehicleDetail }) => {
  const [modal, setModal] = useState(false);
  const [address, setAddress] = useState<IAddress | null>(null);
  const initialFormData = {
    reportVehicleType: 1,
    reportVehicleDescription: "",
    reportVehicleId: 1,
    reportVehicleVIN: vehicleDetail?.vin,
    reportVehicleSPZ: vehicleDetail?.currentLicensePlate,
    reportVehicleBrand: vehicleDetail?.manufacturer,
    reportVehicleModel: vehicleDetail?.model,
    reportVehicleFineAmount: 0,
    reportVehiclePenaltyPoints: 0,
    reportVehiclePaid: false,
    reportVehicleOwnerId: 1,
    reportVehicleOwnerFirstName: vehicleDetail?.ownerFullName.split(" ")[0],
    reportVehicleOwnerLastName: vehicleDetail?.ownerFullName.split(" ")[1],
    reportVehiclePhotos: [] as string[],
  }

  const [formData, setFormData] = useState(initialFormData);
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

        console.log(response);

        if (!response.ok) {
          throw new Error('Failed to load offence types.');
        }
        else {
          const data: IOffenceType[] = await response.json();
          setOffenceTypes(data);
        }

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
    for (let i = 0; i < formData.reportVehiclePhotos.length; i++) {
      let imageBase64 = formData.reportVehiclePhotos[i];
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
          description: formData.reportVehicleDescription,
          personId: formData.reportVehicleOwnerId,
          vehicleId: formData.reportVehicleId,
          fineAmount: formData.reportVehicleFineAmount,
          penaltyPoints: formData.reportVehiclePenaltyPoints,
          finePaid: formData.reportVehiclePaid,
          offenceTypeId: formData.reportVehicleType,
          photos: photosBase64,
          address: address,
        }),
      });

      if (!response.ok) {
        const errP = document.getElementById('reportVehicleErrorMsg');
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
      reportVehicleFineAmount: amount,
      reportVehiclePenaltyPoints: points,
    });
  }

  const handleAddressChange = (address: IAddress) => {
    setAddress(address);
  }

  const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value, type } = event.target;

    if (name === "reportVehicleFineAmount" || name === "reportVehiclePenaltyPoints") {
      setFormData({ ...formData, [name]: parseFloat(value) });
    }
    else if (name === "reportVehiclePhotos") {
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
      console.log(imageFiles);
    }
    else if (type === "checkbox") {
      setFormData({ ...formData, [name]: (event.target as HTMLInputElement).checked });
    }
    else {
      setFormData({ ...formData, [name]: value });
    }

    if (name === "reportVehicleType") {
      const offenceType = offenceTypes.find((offenceType) => offenceType.id === parseInt(value));
      if (offenceType) {
        const speedRow = document.getElementById("reportVehicleSpeedRow");
        if (offenceType.id == 2) {
          // show input for speed
          speedRow?.classList.remove("hidden");
        }
        else {
          // hide input for speed
          speedRow?.classList.add("hidden");
          (document.getElementById("reportVehicleSpeed") as HTMLInputElement)!.value = "0";
        }
        setFormData({
          ...formData,
          reportVehicleType: offenceType.id,
          reportVehicleFineAmount: offenceType.fineAmount,
          reportVehiclePenaltyPoints: offenceType.penaltyPoints,
        });
      }
    }
  }

  return (
    <div>
      <Button color="primary" onClick={toggle} className="me-2"> Nahlásit přestupek </Button>
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
                <Row id="reportVehicleSpeedRow" className="hidden">
                  <Col>
                    <Label>
                      Překročeno o:
                    </Label>
                    <Input id="reportVehicleSpeed" name="reportVehicleSpeed" type="number" step={1} min={0} onChange={handleSpeedChange} />
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
                      Výše pokuty:
                    </Label>
                    <Input id="reportVehicleFineAmount" name="reportVehicleFineAmount" type="number" step={0.01} min={0} value={formData.reportVehicleFineAmount} onChange={handleChange} />
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
                    <Label for="reportVehiclePhotos">
                      Fotky z místa činu:
                    </Label>
                    <Input
                      id="reportVehiclePhotos"
                      name="reportVehiclePhotos"
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
                {/*<Row>*/}
                {/*  <Col>*/}
                {/*    <Label> Rodné číslo: </Label>*/}
                {/*    <Input readOnly id="reportVehicleOwnerBirthNumber" name="reportVehicleOwnerBirthNumber" type="text" value={formData.reportVehicleOwnerBirthNumber} />*/}
                {/*  </Col>*/}
                {/*  <Col>*/}
                {/*    <Label> Datum narození: </Label>*/}
                {/*    <Input readOnly id="reportVehicleOwnerBirthDate" name="reportVehicleOwnerBirthDate" type="text" value={formData.reportVehicleOwnerBirthDate} />*/}
                {/*  </Col>*/}
                {/*</Row>*/}
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
