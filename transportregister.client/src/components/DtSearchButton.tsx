import { Button } from "reactstrap";

const DtSearchButton: React.FC<{
  onSearchClick: () => void,
}> = ({ onSearchClick }) => {
  return (
    <Button
      color="primary"
      onClick={onSearchClick}>
      Vyhledat
    </Button>
  );
}

export default DtSearchButton;
