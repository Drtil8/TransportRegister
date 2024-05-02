import React, { useState } from 'react';
import { Modal, ModalHeader, ModalBody, Row, Col } from 'reactstrap';
import KeyboardArrowRightIcon from '@mui/icons-material/KeyboardArrowRight';
import KeyboardArrowLeftIcon from '@mui/icons-material/KeyboardArrowLeft';
import { IconButton } from '@mui/material';
import { Link } from 'react-router-dom';

interface ImageGalleryProps {
  images: string[];
}

const ImageGallery: React.FC<ImageGalleryProps> = ({ images }) => {
  const [selectedImageIndex, setSelectedImageIndex] = useState<number | null>(null);
  const [modalOpen, setModalOpen] = useState<boolean>(false);

  const openModal = (index: number) => {
    setSelectedImageIndex(index);
    setModalOpen(true);
  };

  const closeModal = () => {
    setModalOpen(false);
  };

  const goToPrevious = () => {
    setSelectedImageIndex(prevIndex => (prevIndex !== null ? (prevIndex + images.length - 1) % images.length : null));
  };

  const goToNext = () => {
    setSelectedImageIndex(prevIndex => (prevIndex !== null ? (prevIndex + 1) % images.length : null));
  };

  return (
    <div className="image-gallery-container">
      <div className="image-grid">
        {images.map((image, index) => (
          <img
            key={index}
            src={`data:image/png;base64,${image}`}
            alt={`Image ${index}`}
            className="image-item"
            onClick={() => openModal(index)}
          />
        ))}
      </div>
      <Modal isOpen={modalOpen} toggle={closeModal}>
        <ModalHeader className="centerSide">
          <Row>
            <Col>
              <IconButton onClick={goToPrevious}>
                <KeyboardArrowLeftIcon />
              </IconButton>
            </Col>
            <Col className="mt-1">
              <Link to="" onClick={closeModal}>
                Zavřít
              </Link>
            </Col>
            <Col>
              <IconButton onClick={goToNext}>
                <KeyboardArrowRightIcon />
              </IconButton>
            </Col>
          </Row>
        </ModalHeader>
        <ModalBody>
          {selectedImageIndex !== null && (
            <img src={`data:image/png;base64,${images[selectedImageIndex]}`} alt="Vybraný" className="modal-image" />
          )}
        </ModalBody>
      </Modal>
    </div>
  );
};

export default ImageGallery;
