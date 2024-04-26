import { MRT_ColumnFiltersState, MRT_SortingState } from "material-react-table";

interface IDtParams {
  start: number;
  size: number;
  filters: MRT_ColumnFiltersState;
  sorting: MRT_SortingState;
}

export default IDtParams;
