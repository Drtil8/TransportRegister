import { Component } from 'react';
import { Link } from 'react-router-dom';
import { Button, Col, Row } from 'reactstrap';
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

    //let API_KEY = "AIzaSyBekkpy5L1p1HJeD-v5i5ZUnuzFk178fZI";
    return (
      <div>
        <Row>
          <Col>
            <h4>Vyhledat řidiče</h4>
          </Col>
        </Row>
        <Row>
          <Col>
            inputy pro vyhledavani
          </Col>
          <Link to="/driver/1">
            <Button color="primary">Example driver detail</Button>
          </Link>
          <br></br>
          {/*<GoogleMapsAutocomplete></GoogleMapsAutocomplete>*/}
        </Row>
      </div>
    );
  }
}

export default DriverSearch;
