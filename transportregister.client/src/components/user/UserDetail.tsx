import { Component } from "react";
import { Alert, Col, Row } from "reactstrap";
import IUserDetail from "../interfaces/IUserDetail";

interface UserDetailProps {
  userDetail: IUserDetail | null;
}

export class UserDetail extends Component<object, UserDetailProps> {
  constructor(props: object) {
    super(props);
    this.state = {
      userDetail: null
    }
  }

  componentDidMount() {
    this.populateUserData();
  }

  async populateUserData() {
    const urlSplitted = window.location.pathname.split('/');
    const id = urlSplitted[2];
    const apiUrl = `/api/User/${id}`;

    try {
      const response = await fetch(apiUrl);

      if (response.ok) {
        const data = await response.json();
        this.setState({ userDetail: data });
      }
      else {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
    }
    catch (error) {
      console.error(error);
    }
  }

  render() {
    const { userDetail } = this.state;

    return (
      <div>
        {this.state.userDetail === null ? (
          <Alert color="danger"> Uživatel nenalezen!</Alert>
        )
          :
          (
            <>
              <h1>Detail uživatele</h1>
              <Row>
                <Col>
                  <dl>
                    <Row>
                      <Col>
                        <dt>Celé jméno:</dt>
                        <dd>{userDetail?.firstName} {userDetail?.lastName}</dd>
                      </Col>
                      <Col>
                        <dt>Email:</dt>
                        <dd>{userDetail?.email}</dd>
                      </Col>
                    </Row>
                    <Row>
                      <Col>
                        <dt>Role:</dt>
                        <dd>{userDetail?.role}</dd>
                      </Col>
                      <Col>
                        <dt>Stav účtu:</dt>
                        <dd>{userDetail?.isValid ? "Aktivní" : "Deaktivovaný"}</dd>
                      </Col>
                    </Row>
                    {userDetail?.role === "Policista" && (
                      <Row>
                        <Col>
                          <dt>Služební číslo:</dt>
                          <dd>{userDetail.personalId}</dd>
                        </Col>
                        <Col>
                          <dt>Hodnost:</dt>
                          <dd>{userDetail.rank}</dd>
                        </Col>
                      </Row>
                    )}
                  </dl>
                </Col>
              </Row>
            </>

          )
        }
      </div>
    );
  }
}
