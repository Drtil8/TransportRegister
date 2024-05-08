import { Button } from "reactstrap";
import IDtFetchData from "./interfaces/datatables/IDtFetchData";

const DtSearchButton: React.FC<{
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>,
}> = ({ fetchDataRef }) => {
  return (
    <Button
      color="primary"
      onClick={() => { fetchDataRef.current?.() }}>
      Vyhledat
    </Button>
  );
}

export default DtSearchButton;
