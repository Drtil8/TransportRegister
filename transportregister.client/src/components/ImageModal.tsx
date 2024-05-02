import { Modal, ModalBody, ModalHeader } from "reactstrap";

interface ImageModalProps {
  isOpen: boolean;
  toggle: () => void;
  image: string;
}

const ImageModal: React.FC<ImageModalProps> = ({ isOpen, toggle, image }) => {
  return (
    <Modal isOpen={isOpen} toggle={toggle}>
      <ModalHeader toggle={toggle}>Fotografie</ModalHeader>
      <ModalBody>
        <img src={image} alt="Fotografie" style={{ width: "100%" }} />
      </ModalBody>
    </Modal>
  );
};

export default ImageModal;
