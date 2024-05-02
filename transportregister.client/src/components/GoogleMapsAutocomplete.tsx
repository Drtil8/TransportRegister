import { useEffect, useState } from "react";
import IAddress from "./interfaces/IAddress";
import { Col, Input, Label, Row } from "reactstrap";


interface GoogleMapsAutocompleteProps {
  onInputChange: (value: IAddress) => void;
}

const GoogleMapsAutocomplete: React.FC<GoogleMapsAutocompleteProps> = ({ onInputChange }) => {

  //const [inputValue, setInputValue] = useState('');
  const [address, setAddress] = useState<IAddress>({
    street: '',
    city: '',
    state: '',
    country: '',
    houseNumber: 0,
    postalCode: 0,
  });

  //const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
  //  const { value } = event.target;
  //  setInputValue(value);
  //  // call the callback 
  //  console.log(value);
  //  onInputChange(value);
  //};

  // Function to update address state
  const updateAddress = (key: keyof IAddress, value: string | number) => {
    setAddress(prevAddress => ({
      ...prevAddress,
      [key]: value,
    }));
    console.log(value);
    const newAddress: IAddress = {
      ...address,
      [key]: value,
    };
    onInputChange(newAddress);
  };

  // using useEffect with no dependancies ([]), so the map is initialized only once
  useEffect(() => {
    let map: google.maps.Map;
    async function initMap(): Promise<void> {
      const { Map } = await google.maps.importLibrary("maps") as google.maps.MapsLibrary;
      map = new Map(document.getElementById("map") as HTMLElement, {
        center: { lat: 49.455391, lng: 17.450310 }, // Lat Lon of Přerov
        zoom: 8,
      });

      const input = document.getElementById("pac-input") as HTMLInputElement;
      const options = {
        fields: ["formatted_address", "geometry", "name", "address_components"],
        componentRestrictions: { country: ["cz", "de", "at", "sk", "pl"] },
        strictBounds: false,
      };

      const autocomplete = new google.maps.places.Autocomplete(input, options);
      autocomplete.addListener("place_changed", () => {
        const place = autocomplete.getPlace();
        if (!place.geometry || !place.geometry.location) {
          console.log("No details available for input: '" + place.name + "'");
          return;
        }
      });

      const marker = new google.maps.Marker({
        map,
        anchorPoint: new google.maps.Point(0, -29),
      });

      autocomplete.addListener("place_changed", () => {
        marker.setVisible(false);
        const place = autocomplete.getPlace();

        if (!place.geometry || !place.geometry.location) {
          console.log("No details available for input: '" + place.name + "'");
          return;
        }

        if (place.geometry.viewport) {
          map.fitBounds(place.geometry.viewport);
        } else {
          map.setCenter(place.geometry.location);
          map.setZoom(10);
        }

        marker.setPosition(place.geometry.location);
        marker.setVisible(true);

        let street = '';
        let city = '';
        let state = '';
        let country = '';
        let houseNumber = '';
        let postalCode = '';


        place.address_components?.forEach((component) => {
          const types = component.types;
          if (types.includes("route")) {
            street = component.long_name;
          } else if (types.includes("administrative_area_level_2")) {
            city = component.long_name;
          } else if (types.includes("administrative_area_level_1")) {
            state = component.long_name;
          } else if (types.includes("country")) {
            country = component.long_name;
          } else if (types.includes("postal_code")) {
            postalCode = component.long_name;
          } else if (types.includes("street_number")) {
            houseNumber = component.long_name;
          }
        });
        const houseNumberNum: number = (houseNumber != '') ? parseInt(houseNumber, 10) : 0;
        const postalCodeNum: number = (postalCode != '') ? parseInt(postalCode, 10) : 0;

        const newAddress: IAddress = {
          street: street,
          city: city,
          state: country,
          country: country,
          houseNumber: houseNumberNum,
          postalCode: postalCodeNum,
        };

        setAddress(newAddress);
      });
    }

    initMap();
  }, []);


  return (

    <div>
      <Input id="pac-input" type="text" placeholder="Enter a location" />
      <br></br>
      <div id="map" style={{ width: '100%', height: '400px' }}></div>
      <div>
        <h2>Adresa:</h2>
        <Row>
          <Col>
            <Label>Země:</Label>
            <Input
              type="text"
              value={address.country}
              onChange={(e) => updateAddress('country', e.target.value)}
            />
          </Col>
          <Col>
            <Label>Kraj:</Label>
            <Input
              type="text"
              value={address.state}
              onChange={(e) => updateAddress('state', e.target.value)}
            />
          </Col>
          <Col>
            <Label>Město:</Label>
            <Input
              type="text"
              value={address.city}
              onChange={(e) => updateAddress('city', e.target.value)}
            />
          </Col>
        </Row>
        <Row>
          <Col xs="12" md="8" lg="8">
            <Label>Ulice:</Label>
            <Input
              type="text"
              value={address.street}
              onChange={(e) => updateAddress('street', e.target.value)}
            />
          </Col>
          <Col xs="6" md="2" lg="2">
            <Label>Číslo domu:</Label>
            <Input
              type="number"
              value={address.houseNumber}
              onChange={(e) => updateAddress('houseNumber', parseInt(e.target.value))}
            />
          </Col>
          <Col xs="6" md="2" lg="2">
            <Label>PSČ:</Label>
            <Input
              type="number"
              value={address.postalCode}
              onChange={(e) => updateAddress('postalCode', parseInt(e.target.value))}
            />
          </Col>
        </Row>
      </div>
    </div>
  );
};

export default GoogleMapsAutocomplete;
