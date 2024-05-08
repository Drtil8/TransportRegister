import { MRT_ColumnFiltersState, MRT_SortingState } from "material-react-table";

interface FilterOptions {
  id: string;
  option: string;
}

interface IDtParams {
  start: number;
  size: number;
  filters: MRT_ColumnFiltersState;
  sorting: MRT_SortingState;
  filterOptions?: FilterOptions[];
}

export default IDtParams;
