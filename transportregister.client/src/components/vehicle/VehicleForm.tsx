import { useState } from 'react';
import { Button, Col, Form, FormGroup, Input, Label, Row } from 'reactstrap';
import { ICar, IVehicleDetail } from '../interfaces/IVehicleDetail';
import { useNavigate } from 'react-router-dom';

export const VehicleForm: React.FC<{ fetchedVehicle: IVehicleDetail | null }> = ({ fetchedVehicle }) => {
  const [selectedVehicleType, setSelectedVehicleType] = useState<string>(
    fetchedVehicle?.vehicleType ?? 'Car');
  const [image, setImage] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    let formData = new FormData(document.getElementById("vehicleCreateForm") as HTMLFormElement);
    let vehicleType = formData.get('vehicleType');
    let additionalFields = {};
    if (vehicleType === 'Car')
      additionalFields = {
        numberOfDoors: formData.get('numberOfDoors') || 0
      };
    else if (vehicleType === 'Motorcycle')
      additionalFields = {
        constraints: formData.get('constraints')
      };
    else if (vehicleType === 'Bus')
      additionalFields = {
        seatCapacity: formData.get('seatCapacity') || 0,
        standingCapacity: formData.get('standingCapacity') || 0
      };

    let imageBase64 = image;
    if (imageBase64) {
      imageBase64 = imageBase64.replace(/^data:image\/[a-z]+;base64,/, "");
    }

    let params = {
      ownerId: 1,
      vehicleId: fetchedVehicle?.vehicleId || 0,
      vehicleType: vehicleType,
      VIN: formData.get('VIN'),
      currentLicensePlate: formData.get('SPZ'),
      manufacturer: formData.get('manufacturer'),
      model: formData.get('model'),
      manufacturedYear: formData.get('manufacturedYear') || 0,
      horsepowerKW: formData.get('horsepowerKW') || 0,
      engineVolumeCM3: formData.get('engineVolumeCM3') || 0,
      loadCapacityKG: formData.get('loadCapacityKG') || 0,
      lengthCM: formData.get('lengthCM') || 0,
      widthCM: formData.get('widthCM') || 0,
      heightCM: formData.get('heightCM') || 0,
      color: formData.get('color'),
      imageBase64: imageBase64,
      ...additionalFields,
    };

    try {
      const response = await fetch(`/api/Vehicle/SaveVehicle`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(params)
      });

      if (response.ok) {
        // navigate to detail
        const json: IVehicleDetail = await response.json();
        navigate(`/vehicle/${json.vehicleId}`);
      }
      else {
        console.error("Create vehicle failed");
      }
    }
    catch (error) {
      console.error('Create vehicle failed: ' + error);
    }
  }

  const handleVehicleTypeChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setSelectedVehicleType(event.target.value);
  };

  const handleImageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files && event.target.files[0];
    if (!file)
      return;
    const reader = new FileReader();
    reader.onloadend = () => {
      const imageInBase64 = reader.result as string;
      setImage(imageInBase64);
    };
    reader.readAsDataURL(file);
  };

  return (
    <Form
      id="vehicleCreateForm"
      encType="multipart/form-data"
      onSubmit={(e) => handleSubmit(e)}>
      <Row>
        <Col>
          <FormGroup>
            <Label for="VIN">VIN:</Label>
            <Input type="text" name="VIN" defaultValue={fetchedVehicle?.vin} required />
          </FormGroup>
        </Col>
        <Col>
          <FormGroup>
            <Label for="SPZ">SPZ:</Label>
            <Input type="text" name="SPZ" defaultValue={fetchedVehicle?.currentLicensePlate} required />
          </FormGroup>
        </Col>
      </Row>

      <Row>
        <Col>
          <FormGroup>
            <Label for="manufacturer">Výrobce:</Label>
            <Input type="text" name="manufacturer" defaultValue={fetchedVehicle?.manufacturer} required />
          </FormGroup>
        </Col>
        <Col>
          <FormGroup>
            <Label for="model">Model:</Label>
            <Input type="text" name="model" defaultValue={fetchedVehicle?.model} required />
          </FormGroup>
        </Col>
      </Row>

      <Row>
        <Col>
          <FormGroup>
            <Label for="horsepowerKW">Výkon (kW):</Label>
            <Input type="number" name="horsepowerKW" min="0" defaultValue={fetchedVehicle?.horsepowerKW} />
          </FormGroup>
        </Col>
        <Col>
          <FormGroup>
            <Label for="engineVolumeCM3">Objem motoru (cm<sup>3</sup>):</Label>
            <Input type="number" name="engineVolumeCM3" min="0" defaultValue={fetchedVehicle?.engineVolumeCM3} />
          </FormGroup>
        </Col>
      </Row>

      <Row>
        <Col>
          <FormGroup>
            <Label for="manufacturedYear">Rok výroby:</Label>
            <Input type="number" name="manufacturedYear" min="1900" defaultValue={fetchedVehicle?.manufacturedYear} />
          </FormGroup>
        </Col>
        <Col>
          <FormGroup>
            <Label for="color">Barva:</Label>
            <Input type="text" name="color" defaultValue={fetchedVehicle?.color} />
          </FormGroup>
        </Col>
        <Col>
          <FormGroup>
            <Label for="loadCapacityKG">Nosnost (kg):</Label>
            <Input type="number" name="loadCapacityKG" min="0" defaultValue={fetchedVehicle?.loadCapacityKG} />
          </FormGroup>
        </Col>
      </Row>

      <Row>
        <Label for="lengthCM">Rozměry (d x š x v) cm:</Label>
        <Col>
          <Input type="number" name="lengthCM" min="0" defaultValue={fetchedVehicle?.lengthCM} />
        </Col>
        <Col>
          <Input type="number" name="widthCM" min="0" defaultValue={fetchedVehicle?.widthCM} />
        </Col>
        <Col>
          <Input type="number" id="heightCM" min="0" defaultValue={fetchedVehicle?.heightCM} />
        </Col>
      </Row>

      <Row>
        <Col xs="6">
          <FormGroup>
            <Label for="vehicleType">Typ vozidla:</Label>
            <Input
              type="select"
              name="vehicleType"
              defaultValue={selectedVehicleType}
              onChange={handleVehicleTypeChange}
              required>
              <option key="Car" value="Car">Auto</option>
              <option key="Truck" value="Truck">Nákladní auto</option>
              <option key="Motorcycle" value="Motorcycle">Motorka</option>
              <option key="Bus" value="Bus">Autobus</option>
            </Input>
          </FormGroup>
        </Col>
      </Row>

      {selectedVehicleType === 'Car' && (
        <Row id="carInputs">
          <Col>
            <FormGroup>
              <Label for="numberOfDoors">Počet dveří:</Label>
              <Input type="number" name="numberOfDoors" min="1"
                defaultValue={fetchedVehicle ? (fetchedVehicle as ICar).numberOfDoors : "5"} />
            </FormGroup>
          </Col>
        </Row>
      )}
      {selectedVehicleType === 'Motorcycle' && (
        <Row id="motorcycleInputs">
          <Col>
            <FormGroup>
              <Label for="constraints">Omezení:</Label>
              <Input type="text" name="constraints" />
            </FormGroup>
          </Col>
        </Row>
      )}
      {selectedVehicleType === 'Bus' && (
        <Row id="busInputs">
          <Label for="constraints">Kapacita (sezení + stání):</Label>
          <Col>
            <Input type="number" name="seatCapacity" min="0" />
          </Col>
          <Col>
            <Input type="number" name="standingCapacity" min="0" />
          </Col>
        </Row>
      )}
      {/*{selectedVehicleType === 'Truck' && (*/}
      {/*)}*/}

      <Row>
        <FormGroup>
          <Label for="vehicleImage">
            Fotka vozidla
          </Label>
          {fetchedVehicle?.imageBase64 && (
            <Row className="mb-2">
              <Col xs="6">
                <img
                  src={`${fetchedVehicle.imageBase64}`}
                  alt="Vozidlo"
                  className="vehicleImage" />
              </Col>
            </Row>
          )}
          <Input
            id="vehicleImage"
            name="ImageBase64"
            type="file"
            accept="image/*"
            onChange={handleImageChange} />
        </FormGroup>
      </Row>

      <Row className="my-3">
        <Col className="d-flex justify-content-center">
          <Button type="submit" size="lg" color="primary">Uložit</Button>
        </Col>
      </Row>
    </Form>
  );
}

export default VehicleForm;
