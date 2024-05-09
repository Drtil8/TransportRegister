import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Alert } from 'reactstrap';
import React from 'react';

interface IAlertModalProps {
  isOpen: boolean;
  toggle: () => void;
  title: string;
  message: string;
  doSomething: () => void;
}

const AlertModal: React.FC<IAlertModalProps> = ({ isOpen, toggle, title, message, doSomething }) => {

  return (
    <Modal isOpen={isOpen} toggle={toggle}>
      <ModalHeader toggle={toggle}>{title}</ModalHeader>
      <ModalBody>
        <Alert className="alert alert-warning">
          {message}
        </Alert>
      </ModalBody>
      <ModalFooter className="d-flex align-items-center justify-content-center">
        <Button color="success" onClick={doSomething}>
          Ano
        </Button>
        <Button color="secondary" onClick={toggle}>
          Ne
        </Button>
      </ModalFooter>
    </Modal>
  );
}

export default AlertModal;
