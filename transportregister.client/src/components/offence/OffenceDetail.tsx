import { Component, ContextType } from 'react';
import { Alert, Button, Col, Input, Row } from 'reactstrap';
import IOffenceDetail from '../interfaces/IOffenceDetail';
import { formatDate, formatDateTime } from '../../common/DateFormatter';
//import LocationPicker from '../location/LocationPicker';
import AuthContext from '../../auth/AuthContext';
import EditIcon from '@mui/icons-material/Edit';
import { Tooltip, IconButton } from '@mui/material';
import SaveIcon from '@mui/icons-material/Save';

interface IOffenceDetailProps {
  offenceDetail: IOffenceDetail | null;
  offenceStateText: string;
  showButtons: boolean;
  editMode: boolean;
  editPenaltyPoints: number;
  editFineAmount: number;
}

// TODO -> zobrazit detail ownera/drivera a vozidla
// Udelat logiku kdy neni zpracovany prestupek a urednik si jej zobrazi
// Nejake hezke zobrazeni zpracovani/nezpracovani pro policistu
export class OffenceDetail extends Component<object, IOffenceDetailProps> {
  static contextType = AuthContext;
  declare context: ContextType<typeof AuthContext>;

  constructor(props: object) {
    super(props);
    this.state = {
      offenceDetail: null,
      offenceStateText: "Kontrolován",
      showButtons: false,
      editMode: false,
      editFineAmount: 0,
      editPenaltyPoints: 0
    }
    this.handleDecline = this.handleDecline.bind(this);
    this.handleApprove = this.handleApprove.bind(this);
    this.handleEditButton = this.handleEditButton.bind(this);
    this.handleSaveButton = this.handleSaveButton.bind(this);
    this.handlePayFine = this.handlePayFine.bind(this);
  }

  async handleSaveButton() {
    // todo -> show new values
    this.setState({ offenceDetail: { ...this.state.offenceDetail, penaltyPoints: this.state.editPenaltyPoints } });
    //this.setState({ offenceDetail: { ...this.state.offenceDetail, fine: { ...this.state.offenceDetail.fine, amount: this.state.editFineAmount } });

    try {
      const response = await fetch(`/api/Offence/${this.state.offenceDetail?.offenceId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ penaltyPoints: this.state.editPenaltyPoints, fineAmount: this.state.editFineAmount })
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
    }
    catch (error) {
      console.error('Error:', error);
    }

    this.setState({ editMode: false });
    document.getElementById("editButton")?.classList.remove("hidden");
    document.getElementById("saveButton")?.classList.add("hidden");
  }

  handleEditButton() { // todo -> hidden gets errors in dev mode in browser console
    this.setState({ editMode: true });
    document.getElementById("editButton")?.classList.add("hidden");
    document.getElementById("saveButton")?.classList.remove("hidden");
  }

  async handlePayFine() { // TODO -> modal
    // todo -> show new values
    try {
      const response = await fetch(`/api/Offence/${this.state.offenceDetail?.offenceId}/PayFine`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        }
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
    }
    catch (error) {
      console.error('Error:', error);
    }
  }


  async handleDecline() { // TODO -> modal
    console.log("Decline");
    try {
      const response = await fetch(`/api/Offence/${this.state.offenceDetail?.offenceId}/Decline`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        }
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      else {
        this.setState({ showButtons: false, offenceStateText: "Neschválen" });

      }
    }
    catch (error) {
      console.error('Error:', error);
    }
  }

  async handleApprove() { // TODO -> modal
    // todo editable input in <dd> -> gpt
    // todo map to backend
    try {
      // fetch - todo
      const response = await fetch(`/api/Offence/${this.state.offenceDetail?.offenceId}/Approve`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        }
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      else {
        this.setState({ offenceStateText: "Schválen" });
        this.setState({ showButtons: false });
      }
    }
    catch (error) {
      console.error('Error:', error);
    }

    this.setState({ showButtons: false });
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
      if (data.isApproved && data.isValid) {
        this.setState({ offenceStateText: "Schválen" });
      }
      else if (!data.isApproved && data.isValid) {
        this.setState({ offenceStateText: "Rozpracován" });
        // TODO -> think about this logic -> if official goes to vacation other official cannot process his offences

        if (this.context?.isOfficial) { // TODO -> here any official can process the offence
          this.setState({ showButtons: true });
        }

        //if (data.isResponsibleOfficial) {
        //  this.setState({ showButtons: true });
        //}
      }
      else {
        this.setState({ offenceStateText: "Neschválen" });
      }
      this.setState({ editPenaltyPoints: data.penaltyPoints });
      this.setState({ editFineAmount: data.fine?.amount ? data.fine.amount : 0 });
    }
    catch (error) {
      console.error('Error:', error);
    }
  }

  render() {
    const { offenceDetail, offenceStateText, editMode } = this.state;
    const { editPenaltyPoints, editFineAmount } = this.state;

    const contetnt = !offenceDetail ?
      (
        <Alert color="danger"> Přestupek nebyl nalezen. </Alert>
      )
      :
      (
        <Row>
          {/*<Col xs="10">*/}
          <Col>
            <Row>
              <Col>
                <h4>Přestupek - P.{offenceDetail.offenceId} - {offenceStateText}</h4>
              </Col>
              {this.state.showButtons && (
                <Col>
                  <Row>
                    {/*TODO ->errors in dev mode in browser console*/}
                    <Col id="editButton">
                      <Tooltip title="Upravit přestupek">
                        <IconButton color="primary" size="large" onClick={this.handleEditButton}>
                          <EditIcon fontSize="inherit" />
                        </IconButton>
                      </Tooltip>
                    </Col>
                    <Col className="hidden" id="saveButton">
                      <Tooltip title="Uložit úpravy">
                        <IconButton color="primary" size="large" onClick={this.handleSaveButton}>
                          <SaveIcon />
                        </IconButton>
                      </Tooltip>
                    </Col>
                  </Row>
                </Col>
              )}
            </Row>
            <Row>
              <dl>
                <Row>
                  <dt>Popis:</dt>
                  <dd><textarea readOnly value={offenceDetail.description ? offenceDetail.description : ""} className="form-control" /></dd>
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
                    {editMode ? (
                      <Input type="number" id="offenceDetailPenaltyPoints" name="offenceDetailPenaltyPoints" value={editPenaltyPoints} onChange={(e) => this.setState({ editPenaltyPoints: parseInt(e.target.value) })}></Input>
                    )
                      :
                      (

                        <dd>{offenceDetail.penaltyPoints}</dd>
                      )}
                  </Col>
                </Row>
                {offenceDetail.fine !== null && (
                  <div>
                    <hr />
                    <Row>
                      <h4> Pokuta </h4>
                    </Row>

                    {offenceDetail.fine.isPaid === true ?
                      (
                        <Row>
                          <Col>
                            <dt>Zaplaceno</dt>
                            <dd className="yes"> <b>ANO</b> </dd>
                          </Col>
                          <Col>
                            <dt>Datum zaplacení</dt>
                            <dd className="yes">{formatDate(offenceDetail.fine.paidOn)}</dd>
                          </Col>
                        </Row>
                      )
                      :
                      (
                        <Row>
                          <Col>
                            <dt>Zaplaceno</dt>
                            <dd className="no"> <b>NE</b> </dd>
                          </Col>
                          <Col>
                            <dt>Datum splatnosti</dt>
                            <dd className="no">{formatDate(offenceDetail.fine.dueDate)}</dd>
                          </Col>
                        </Row>
                      )}
                    <Row>
                      <Col>
                        <dt>Výše pokuty:</dt>
                        {editMode ? (
                          <Input type="number" id="offenceDetailFineAmount" name="offenceDetailFineAmount" value={editFineAmount} onChange={(e) => this.setState({ editFineAmount: parseInt(e.target.value) })}></Input>
                        )
                          :
                          (
                            <dd>{offenceDetail.fine.amount} Kč</dd>
                          )}
                      </Col>
                      {offenceDetail.fine.isPaid === false && (
                        <Col>
                          <Button color="primary" onClick={this.handlePayFine}> Potvrdit zaplacení </Button>
                        </Col>
                      )}
                    </Row>
                  </div>
                )}
              </dl>
            </Row>
            <Row>
              <hr />
              <Col>
                <h4>Osoba</h4>
                <Row>
                  <Col>
                    <dt>Jméno</dt>
                    <dd>TODO</dd>
                  </Col>
                  <Col>
                    <dt>Příjmení</dt>
                    <dd>TODO</dd>
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <dt>Rodné číslo</dt>
                    <dd>TODO</dd>
                  </Col>
                  <Col>
                    <dt>todo</dt>
                    <dd>TODO</dd>
                  </Col>
                </Row>
              </Col>
              {offenceDetail.vehicle && (
                <Col>
                  <h4>Vozidlo</h4>
                  <Row>
                    <Col>
                      <dt>VIN</dt>
                      <dd>{offenceDetail.vehicle.vin}</dd>
                    </Col>
                    <Col>
                      <dt>SPZ</dt>
                      <dd>{offenceDetail.vehicle.licensePlate}</dd>
                    </Col>
                  </Row>
                  <Row>
                    <Col>
                      <dt>Výrobce</dt>
                      <dd>{offenceDetail.vehicle.manufacturer}</dd>
                    </Col>
                    <Col>
                      <dt>Model</dt>
                      <dd>{offenceDetail.vehicle.model}</dd>
                    </Col>
                  </Row>
                </Col>
              )}
            </Row>
            {this.state.showButtons && (
              <Row className="mt-4">
                <hr />
                <Col className="d-flex justify-content-end pe-0">
                  <Button color="success" className="me-2" onClick={this.handleApprove}>Schválit</Button>
                  <Button color="danger" className="me-0" onClick={this.handleDecline}>Zamítnout</Button>
                </Col>
              </Row>
            )}
          </Col>
          {/*<Col>*/}
          {/*  panel operací*/}
          {/*</Col>*/}
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
