import { Component } from 'react';
import { Link } from 'react-router-dom';
import { Button, Col, Row } from 'reactstrap';
//import VehicleDatatable from './vehicle/VehicleDatatable';
import IDtFetchData from './interfaces/datatables/IDtFetchData';
import DriverDatatable from './DriverDatatable';
import IAddress from './interfaces/IAddress';

// googleAdress JE PRIKLAD PRO VYUZITI GoogleMapsAutocomplete
interface IDriverSearchState {
  googleAdress: IAddress | null;
}
export class DriverSearch extends Component<object, IDriverSearchState> {

  constructor(props: object) {
    super(props);
    this.state = {
      googleAdress: null as IAddress | null,
    };
  }
  fetchDataRef: React.MutableRefObject<IDtFetchData | null> = { current: null };

  handleInputChange = (value: IAddress) => {
    this.setState({ googleAdress: value });
    console.log('parent', value)
  };

  render() {

    return (
      <div>
        <Row>
          <Col>
            <h4>Vyhledat řidiče</h4>
          </Col>
        </Row>
        <Row>
          {/*<GoogleMapsAutocomplete onInputChange={this.handleInputChange}></GoogleMapsAutocomplete>*/}
          <DriverDatatable fetchDataRef={this.fetchDataRef} autoFetch={true} />
        </Row>
      </div>
    );
  }
}

export default DriverSearch;
