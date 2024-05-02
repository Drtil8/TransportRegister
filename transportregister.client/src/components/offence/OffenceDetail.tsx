import { Component, ContextType } from 'react';
import { Alert, Button, Col, Input, Row } from 'reactstrap';
import IOffenceDetail from '../interfaces/IOffenceDetail';
import { formatDate, formatDateTime } from '../../common/DateFormatter';
import AuthContext from '../../auth/AuthContext';
import { IconButton } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import DetailIcon from '@mui/icons-material/VisibilityOutlined';
import { Link } from 'react-router-dom';
import ImageGallery from '../ImageGallery';

interface IOffenceDetailProps {
  offenceDetail: IOffenceDetail | null;
  offenceStateText: string;
  offenceStateColor: string;
  showButtons: boolean;
  editMode: boolean;
  editPenaltyPoints: number;
  editFineAmount: number;
}

export class OffenceDetail extends Component<object, IOffenceDetailProps> {
  static contextType = AuthContext;
  declare context: ContextType<typeof AuthContext>;

  constructor(props: object) {
    super(props);
    this.state = {
      offenceDetail: null,
      offenceStateText: "Rozpracován",
      offenceStateColor: "no",
      showButtons: false,
      editMode: false,
      editFineAmount: 0,
      editPenaltyPoints: 0
    }
    this.handleDecline = this.handleDecline.bind(this);
    this.handleApprove = this.handleApprove.bind(this);
    this.handleEditButton = this.handleEditButton.bind(this);
    this.handleSaveButton = this.handleSaveButton.bind(this);
    this.handleEmptyInput = this.handleEmptyInput.bind(this);
  }

  async handleSaveButton() {
    this.setState({ offenceDetail: { ...this.state.offenceDetail!, penaltyPoints: this.state.editPenaltyPoints } });

    if (this.state.offenceDetail?.fine !== null) {
      this.setState({ offenceDetail: { ...this.state.offenceDetail!, fine: { ...this.state.offenceDetail!.fine!, amount: this.state.editFineAmount } } });
    }

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

      this.populateOffenceData();
    }
    catch (error) {
      console.error('Error:', error);
    }

    this.setState({ editMode: false });
    document.getElementById("editButton")?.classList.remove("hidden");
    document.getElementById("saveButton")?.classList.add("hidden");
    document.getElementById("offenceDetailTextArea")?.setAttribute("readonly", "true");
  }

  handleEditButton() {
    this.setState({ editMode: true });
    document.getElementById("editButton")?.classList.add("hidden");
    document.getElementById("saveButton")?.classList.remove("hidden");
    document.getElementById("offenceDetailTextArea")?.removeAttribute("readonly")
  }

  handleEmptyInput(e: any) {
    if (e.target.value === "") {
      e.target.value = 0;
      if (e.target.name === "offenceDetailPenaltyPoints") {
        this.setState({ editPenaltyPoints: 0 });
      }
      else {
        this.setState({ editFineAmount: 0 });
      }
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
        this.setState({ showButtons: false, offenceStateText: "Neschválen", offenceStateColor: "no" });

      }
    }
    catch (error) {
      console.error('Error:', error);
    }
  }

  async handleApprove() {
    try {
      const response = await fetch(`/api/Offence/${this.state.offenceDetail?.offenceId}/Approve`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(this.state.offenceDetail)
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      else {
        const data = await response.json();
        this.setState({ offenceDetail: data })
        this.setState({ showButtons: false, offenceStateText: "Schválen", offenceStateColor: "yes" });
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
        this.setState({ offenceStateText: "Schválen", offenceStateColor: "yes" });
      }
      else if (!data.isApproved && data.isValid) {
        this.setState({ offenceStateText: "Rozpracován", offenceStateColor: "workedOn" });
        if (this.context?.isOfficial) { // TODO -> here any official can process the offence
          this.setState({ showButtons: true });
        }

        //if (data.isResponsibleOfficial) {
        //  this.setState({ showButtons: true });
        //}
      }
      else {
        this.setState({ offenceStateText: "Neschválen", offenceStateColor: "no" });
      }
      this.setState({ editPenaltyPoints: data.penaltyPoints });
      this.setState({ editFineAmount: data.fine?.amount ? data.fine.amount : 0 });
    }
    catch (error) {
      console.error('Error:', error);
    }
  }

  render() {
    const { offenceDetail, offenceStateText, offenceStateColor, editMode } = this.state;
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
                <h4>Přestupek - P.{offenceDetail.offenceId} - <b className={offenceStateColor}>{offenceStateText}</b></h4>
              </Col>
              {this.state.showButtons && (
                <Col className="rightSide">
                  <Row>
                    {/*TODO ->errors in dev mode in browser console*/}
                    <Col id="editButton">
                      {/*<Tooltip title="Upravit přestupek">*/}
                      <IconButton color="primary" size="large" className="pt-0 pe-0" onClick={this.handleEditButton}>
                        <EditIcon fontSize="inherit" />
                      </IconButton>
                      {/*</Tooltip>*/}
                    </Col>
                    <Col className="hidden" id="saveButton">
                      {/*<Tooltip title="Uložit úpravy">*/}
                      <IconButton color="primary" size="large" className="pt-0 pe-0" onClick={this.handleSaveButton}>
                        <SaveIcon />
                      </IconButton>
                      {/*</Tooltip>*/}
                    </Col>
                  </Row>
                </Col>
              )}
            </Row>
            <Row>
              <dl>
                <Row>
                  <dt>Popis:</dt>
                  <dd><textarea id="offenceDetailTextArea" readOnly value={offenceDetail.description ? offenceDetail.description : ""} className="form-control" /></dd>
                </Row>
                <Row>
                  <Col>
                    <dt>Typ přestupku:</dt>
                    <dd>{offenceDetail.type}</dd>
                  </Col>
                  <Col>
                    <dt>Trestné body:</dt>
                    {editMode ? (
                      <Input type="number" id="offenceDetailPenaltyPoints" name="offenceDetailPenaltyPoints" value={editPenaltyPoints} onChange={(e) => this.setState({ editPenaltyPoints: parseInt(e.target.value) })} max={12} min={0} onBlur={this.handleEmptyInput}></Input>
                    )
                      :
                      (
                        <dd>{offenceDetail.penaltyPoints}</dd>
                      )}
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <dt>Nahlášen</dt>
                    <dd>{formatDateTime(offenceDetail.reportedOn)}</dd>
                  </Col>
                  <Col>
                    <dt>
                      Nahlásil:
                    </dt>
                    <dd>
                      <Link to={`/user/${offenceDetail.officer.id}`}>
                        {offenceDetail.officer.fullName}
                      </Link>
                    </dd>
                  </Col>
                </Row>
                <Row>
                  <Col>
                    <dt>Místo činu:</dt>
                    <dd>{offenceDetail.address}</dd>
                  </Col>
                </Row>
                {offenceDetail.offencePhotos64 && offenceDetail.offencePhotos64.length > 0 && (
                  <Row>
                    <Col>
                      <dt>Fotky z místa činu:</dt>
                      <ImageGallery images={offenceDetail.offencePhotos64}></ImageGallery>
                    </Col>
                  </Row>
                )}
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
                          <Input type="number" id="offenceDetailFineAmount" name="offenceDetailFineAmount" value={editFineAmount} onChange={(e) => this.setState({ editFineAmount: parseInt(e.target.value) })} min={0} onBlur={this.handleEmptyInput}></Input>
                        )
                          :
                          (
                            <dd>{offenceDetail.fine.amount} Kč</dd>
                          )}
                      </Col>
                    </Row>
                  </div>
                )}
              </dl>
            </Row>
            <Row>
              <hr />
              <Col>
                <Row>
                  <Col >
                    <h4>Osoba</h4>
                  </Col>
                  <Col>
                    <Link to={`/driver/${offenceDetail.person.personId}`}>
                      <DetailIcon />
                    </Link>
                  </Col>
                </Row>
                <Row>
                  <Col sm="6">
                    <dt>Celé jméno</dt>
                    <dd>{offenceDetail.person.fullName}</dd>
                  </Col>
                  <Col sm="6">
                    <dt>Rodné číslo</dt>
                    <dd>{offenceDetail.person.birthNumber}</dd>
                  </Col>
                </Row>
                {/*<Row>*/}
                {/*  <Col>*/}
                {/*    <dt>todo</dt>*/}
                {/*    <dd>TODO</dd>*/}
                {/*  </Col>*/}
                {/*  <Col>*/}
                {/*    <dt>todo</dt>*/}
                {/*    <dd>TODO</dd>*/}
                {/*  </Col>*/}
                {/*</Row>*/}
              </Col>
              {offenceDetail.vehicle && (
                <Col>
                  <Row>
                    <Col>
                      <h4>Vozidlo</h4>
                    </Col>
                    <Col>
                      <Link to={`/vehicle/${offenceDetail.vehicle.vehicleId}`}>
                        <DetailIcon />
                      </Link>
                    </Col>
                  </Row>
                  <Row>
                    <Col sm="6">
                      <dt>VIN</dt>
                      <dd>{offenceDetail.vehicle.vin}</dd>
                    </Col>
                    <Col sm="6">
                      <dt>SPZ</dt>
                      <dd>{offenceDetail.vehicle.licensePlate}</dd>
                    </Col>
                  </Row>
                  <Row>
                    <Col sm="6">
                      <dt>Výrobce</dt>
                      <dd>{offenceDetail.vehicle.manufacturer}</dd>
                    </Col>
                    <Col sm="6">
                      <dt>Model</dt>
                      <dd>{offenceDetail.vehicle.model}</dd>
                    </Col>
                  </Row>
                </Col>
              )}
            </Row>
            {this.state.showButtons ? (
              <Row className="mt-4">
                <hr />
                <Col className="rightSide pe-0">
                  <Button color="success" className="me-2" onClick={this.handleApprove}>Schválit</Button>
                  <Button color="danger" className="me-0" onClick={this.handleDecline}>Zamítnout</Button>
                </Col>
              </Row>
            )
              :
              (
                <Row className="mt-4">
                  <hr />
                  <Col className="pe-0">
                    <dt className="mb-1">Zpracoval:</dt>
                    {offenceDetail.official ?
                      <dd>
                        <Link to={`/user/${offenceDetail.official?.id}`}>
                          {offenceDetail.official?.fullName}
                        </Link>
                      </dd>
                      :
                      <dd>Systém</dd>
                    }
                  </Col>
                </Row>
              )}
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
