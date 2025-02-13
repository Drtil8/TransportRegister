﻿import React, { FormEvent, useState } from "react";
import { Button, Col, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader, Row } from "reactstrap";
import GoogleMapsAutocomplete from "../GoogleMapsAutocomplete";
import IAddress from "../interfaces/IAddress";
import { IVehicleDetail } from "../interfaces/IVehicleDetail";
import { useNavigate } from "react-router-dom";

interface TheftReportModalProps {
  vehicleDetail: IVehicleDetail | null;
}

const TheftReportModal: React.FC<TheftReportModalProps> = ({ vehicleDetail }) => {
  const navigate = useNavigate();
  const [modal, setModal] = useState(false);
  const [address, setAddress] = useState<IAddress | null>(null);
  const initialFormData = {
    reportTheftVehicleId: vehicleDetail?.vehicleId,
    reportTheftStolenOn: "",
    reportTheftDescription: "",
    reportTheftVIN: vehicleDetail?.vin,
    reportTheftSPZ: vehicleDetail?.currentLicensePlate,
    reportTheftBrand: vehicleDetail?.manufacturer,
    reportTheftModel: vehicleDetail?.model,
    reportTheftOwnerId: vehicleDetail?.ownerId,
    reportTheftOwnerFirstName: vehicleDetail?.ownerFullName.split(" ")[0],
    reportTheftOwnerLastName: vehicleDetail?.ownerFullName.split(" ")[1],
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
          address: address,
          vehicleId: formData.reportTheftVehicleId,
          reportingPersonId: formData.reportTheftOwnerId,
        }),
      });

      if (!response.ok) {
        const errP = document.getElementById("reportTheftErrorMsg");
        errP?.classList.remove("hidden");
        errP!.innerText = await response.text();
      }
      else {
        toggle();
        const theftId = await response.text();
        navigate('/theft/' + theftId);
      }
    }
    catch (error) {
      console.error(error);
    }
  };

  const handleAddressChange = (address: IAddress) => {
    setAddress(address);
  }

  const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = event.target;
    setFormData({ ...formData, [name]: value });
  }

  return (
    <div>
      <Button color="danger" onClick={toggle}>Nahlásit krádež</Button>
      <Modal isOpen={modal} toggle={toggle} backdrop="static">
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
                    <Input id="reportTheftStolenOn" name="reportTheftStolenOn" type="datetime-local" value={formData.reportTheftStolenOn} onChange={handleChange} required />
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
                  <b>Ukradeno osobě:</b>
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
                {/*<Row>*/}
                {/*  <Col>*/}
                {/*    <Label> Rodné číslo: </Label>*/}
                {/*    <Input readOnly id="reportTheftOwnerBirthNumber" name="reportTheftOwnerBirthNumber" type="text" value={formData.reportTheftOwnerBirthNumber} />*/}
                {/*  </Col>*/}
                {/*  <Col>*/}
                {/*    <Label> Datum narození: </Label>*/}
                {/*    <Input readOnly id="reportTheftOwnerBirthDate" name="reportTheftOwnerBirthDate" type="text" value={formData.reportTheftOwnerBirthDate} />*/}
                {/*  </Col>*/}
                {/*</Row>*/}
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
