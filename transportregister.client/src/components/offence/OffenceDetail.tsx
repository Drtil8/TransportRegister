import { Component } from 'react';
import { Alert, Col, Row } from 'reactstrap';
import IOffenceDetail from '../interfaces/IOffenceDetail';
import { formatDate, formatDateTime } from '../../common/DateFormatter';
//import LocationPicker from '../location/LocationPicker';

interface IOffenceDetailProps {
  offenceDetail: IOffenceDetail | null;
}

// TODO -> zobrazit detail ownera/drivera a vozidla
// Udelat logiku kdy neni zpracovany prestupek a urednik si jej zobrazi
// Nejake hezke zobrazeni zpracovani/nezpracovani pro policistu
export class OffenceDetail extends Component<object | IOffenceDetailProps> {
  constructor(props: object) {
    super(props);
    this.state = {
      offenceDetail: null
    }
  }

  componentDidMount() {
    this.populateOffenceData();
  }

  async populateOffenceData() {
    // TODO
    const urlSplitted = window.location.pathname.split('/');
    const id = urlSplitted[2];
    const apiUrl = `/api/Offence/${id}`;

    try {
      const response = await fetch(apiUrl);

      if (!response.ok) {
        throw new Error(`HTTP error!`);
      }

      const data: IOffenceDetail = await response.json();
      this.setState({ offenceDetail: data });
      console.log(data);
    }
    catch (error) {
      console.error('Error:', error);
    }
  }

  render() {
    const { offenceDetail } = this.state as IOffenceDetailProps;

    const contetnt = !offenceDetail ?
      (
        <Alert color="danger"> Přestupek nebyl nalezen. </Alert>
      )
      :
      (
        <Row>
          <Col xs="10">
            <Row>
              <Col>
                <h4>Detail přestupku - P.{offenceDetail.offenceId}</h4>
              </Col>
            </Row>
            <Row>
              <dl>
                <Row>
                  <dt>Popis:</dt>
                  <dd><textarea readOnly value={offenceDetail.description} className="form-control" /></dd>
                </Row>
                <Row>
                  <Col>
                    <dt>Nahlášen</dt>
                    <dd>{formatDateTime(offenceDetail.reportedOn)}</dd>
                  </Col>
                  <Col>
                    <dt>Místo činu:</dt>
                    <dd>adresa</dd>
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <dt>Typ přestupku:</dt>
                    <dd>{offenceDetail.type}</dd>
                  </Col>
                  <Col>
                    <dt>Trestné body:</dt>
                    <dd>{offenceDetail.penaltyPoints}</dd>
                  </Col>
                </Row>
                {offenceDetail.fine !== null && (
                  <div>
                    <Row>
                      <h4> Pokuta </h4>
                    </Row>

                    {offenceDetail.fine.isPaid === true ?
                      (
                        <Row>
                          <Col>
                            <dt>Zaplaceno</dt>
                            <dd> <b>ANO</b> </dd>
                          </Col>
                          <Col>
                            <dt>Datum zaplacení</dt>
                            <dd>{formatDate(offenceDetail.fine.paidOn)}</dd>
                          </Col>
                        </Row>
                      )
                      :
                      (
                        <Row>
                          <Col>
                            <dt>Zaplaceno</dt>
                            <dd> <b>NE</b> </dd>
                          </Col>
                          <Col>
                            <dt>Datum splatnosti</dt>
                            <dd>{formatDate(offenceDetail.fine.dueDate)}</dd>
                          </Col>
                        </Row>
                      )}
                    <Row>
                      <Col>
                        <dt>Výše pokuty:</dt>
                        <dd>{offenceDetail.fine.amount} Kč</dd>
                      </Col>
                    </Row>
                  </div>
                )}
              </dl>
            </Row>
          </Col>
          <Col>
            panel operací
          </Col>
        </Row>
      );

    return (
      <div className="container-fluid">
        {contetnt}
      </div>
    );
  }
}

export default OffenceDetail;
