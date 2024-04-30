import React from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Alert } from 'reactstrap';

interface IUserDeleteModalProps {
  userId: string | null;
  isOpen: boolean;
  toggle: () => void;
  fetchData: () => void;
}


const UserDeleteModal: React.FC<IUserDeleteModalProps> = ({ userId, isOpen, toggle, fetchData }) => {

  const delteUser = async () => {
    try {
      const response = await fetch(`api/User/${userId}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json'
        }
      });

      if (response.ok) {
        fetchData(); // Fetch data again
        toggle();
      }
      else {
        console.error(response.statusText);
      }
    }
    catch (error) {
      console.error(error);
    }
  }


  return (
    <Modal isOpen={isOpen} toggle={toggle}>
      <ModalHeader toggle={toggle}>Vymazání projektu</ModalHeader>
      <ModalBody>
        <Alert className="alert alert-warning">
          Opravdu chcete vymazat uživatele?
          Uživatelský účet bude znevalidněn a nebude možné se s ním přihlásit.
          Tato operace jde vrátit.
        </Alert>
      </ModalBody>
      <ModalFooter className="d-flex align-items-center justify-content-center">
        <Button color="danger" onClick={delteUser}>
          Vymazat
        </Button>
        <Button color="secondary" onClick={toggle}>
          Zrušit
        </Button>
      </ModalFooter>
    </Modal>
  );
}

export default UserDeleteModal;
