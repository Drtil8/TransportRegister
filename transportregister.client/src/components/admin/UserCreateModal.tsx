import React, { useState } from 'react';
import { Form, Button, Modal, ModalHeader, ModalBody, ModalFooter, Row, FormGroup, Label, Input, Col } from 'reactstrap';

interface CreateUserModalProps {
  //fetchDataRef: React.MutableRefObject<any>; // TODO
}

const UserCreateModal: React.FC<CreateUserModalProps> = () => {
  const [modal, setModal] = useState(false);
  const toggle = () => setModal(!modal);


  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const password = (document.getElementById("createUserPassword") as HTMLInputElement).value;
    const passwordAgain = (document.getElementById("createUserPasswordAgain") as HTMLInputElement).value;
    if (password !== passwordAgain) {
      const error = document.getElementById("error");
      error!.classList.remove("hidden");
      error!.innerText = "* Hesla se neshodují";
      return;
    }

    // TODO -> password encryption when sending
    try {
      const response = await fetch('/api/User', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          email: (document.getElementById("createUserEmail") as HTMLInputElement).value,
          firstname: (document.getElementById("createUserFirstname") as HTMLInputElement).value,
          lastname: (document.getElementById("createUserLastname") as HTMLInputElement).value,
          role: (document.getElementById("createUserRole") as HTMLInputElement).value,
          personalNumber: (document.getElementById("createUserPersonalNumber") as HTMLInputElement).value,
          rank: (document.getElementById("createUserRank") as HTMLInputElement).value,
          password: (document.getElementById("createUserPassword") as HTMLInputElement).value,
          passwordConfirm: (document.getElementById("createUserPasswordAgain") as HTMLInputElement).value
        })
      });

      if (response.ok) {
        //const fetchData = fetchDataRef.current;
        //fetchData();
        toggle();
      }
      else {
        const error = document.getElementById("error");
        error!.classList.remove("hidden");
        error!.innerText = await response.text();
      }
    }
    catch (error) {
      console.error(error);
    }
  }

  const handleSelectChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { value } = event.target;

    const officerRow = document.getElementById("createUserOfficerRow");
    if (value === "officer") {
      officerRow?.classList.remove("hidden");
    }
    else {
      officerRow?.classList.add("hidden");
    }
  }

  return (
    <div>
      <Button color="success" onClick={toggle}>
        Přidat uživatele
      </Button>
      <Modal isOpen={modal} toggle={toggle}>
        <ModalHeader toggle={toggle}>Tvorba uživatele</ModalHeader>
        <Form id="createUserForm" onSubmit={handleSubmit}>
          <ModalBody>
            <FormGroup>
              <Row>
                <Col>
                  <Label> Email:</Label>
                  <Input required id="createUserEmail" name="createUserEmail" type="email" />
                </Col>
              </Row>
              <Row className="mt-2">
                <Col>
                  <Label> Jméno:</Label>
                  <Input required id="createUserFirstname" name="createUserFirstname" type="text" />
                </Col>
                <Col>
                  <Label> Příjmení:</Label>
                  <Input required id="createUserLastname" name="createUserLastname" type="text" />
                </Col>
              </Row>
              <Row className="mt-2">
                <Col>
                  <Label> Typ účtu:</Label>
                  <Input required id="createUserRole" name="createUserRole" type="select" onChange={handleSelectChange}>
                    <option value="Official">Úředník</option>
                    <option value="Officer">Policista</option>
                    <option value="Admin">Administrátor</option>
                  </Input>
                </Col>
              </Row>
              <Row id="createUserOfficerRow" className="hidden mt-2">
                <Col>
                  <Label> Osobní číslo:</Label>
                  <Input id="createUserPersonalNumber" name="createUserPersonalNumber" type="text" />
                </Col>
                <Col>
                  <Label>Hodnost</Label>
                  <Input id="createUserRank" name="createUserRank" type="text" />
                </Col>
              </Row>
              <Row>
                <Col>
                  <Label>Heslo:</Label>
                  <Input id="createUserPassword" name="createUserPassword" type="password"></Input>
                </Col>
              </Row>
              <Row>
                <Col>
                  <Label>Heslo znovu:</Label>
                  <Input id="createUserPasswordAgain" name="createUserPasswordAgain" type="password"></Input>
                </Col>
              </Row>
              <Row>
                <p id="error" className="text-danger hidden"></p>
              </Row>
            </FormGroup>
          </ModalBody>
          <ModalFooter>
            <Button color="success" type="submit">
              Vytvořit
            </Button>
            <Button color="secondary" onClick={toggle}>
              Zrušit
            </Button>
          </ModalFooter>
        </Form>
      </Modal>
    </div>
  );
}

export default UserCreateModal;
