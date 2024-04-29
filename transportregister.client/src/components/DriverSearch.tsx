import { Component } from 'react';
import { Link } from 'react-router-dom';
import { Button, Col, Row } from 'reactstrap';
import GooglePlacesAutocomplete from 'react-google-places-autocomplete';
//import { APIProvider, Map } from '@vis.gl/react-google-maps';
export class DriverSearch extends Component<object> {
  //componentDidMount() {
  //  const API_KEY = "AIzaSyBekkpy5L1p1HJeD-v5i5ZUnuzFk178fZI";
  //  const script = document.createElement("script");
  //  script.src = `https://maps.googleapis.com/maps/api/js?key=${API_KEY}&libraries=places`;
  //  script.async = true;
  //  document.body.appendChild(script);
  //}
  constructor(props: object) {
    super(props);
  }

  render() {

    let API_KEY = "AIzaSyBekkpy5L1p1HJeD-v5i5ZUnuzFk178fZI";
    return (
      <div>
        <Row>
          <Col>
            <h4>Vyhledat řidiče</h4>
          </Col>
        </Row>
        {/*<ProjectsList fetchUrl={'/api/RiskProject/UserRiskProjects'} />*/}
        <Row>
          <Col>
            inputy pro vyhledavani
          </Col>
          <Link to="/driver/1">
            <Button color="primary">Example driver detail</Button>
          </Link>
          {/*<APIProvider apiKey={API_KEY}>*/}
          {/*  <Map*/}
          {/*    style={{ width: '100vw', height: '100vh' }}*/}
          {/*    defaultCenter={{ lat: 22.54992, lng: 0 }}*/}
          {/*    defaultZoom={3}*/}
          {/*    gestureHandling={'greedy'}*/}
          {/*    disableDefaultUI={true}*/}
          {/*  />*/}
          {/*</APIProvider>*/}
          <div>
            <GooglePlacesAutocomplete
              apiKey="AIzaSyBekkpy5L1p1HJeD-v5i5ZUnuzFk178fZI"
            />
          </div>
        </Row>
      </div>
    );
  }
}

export default DriverSearch;
