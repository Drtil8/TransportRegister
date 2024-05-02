import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  MaterialReactTable,
  useMaterialReactTable,
  type MRT_ColumnDef,
  type MRT_ColumnFiltersState,
  type MRT_PaginationState,
  type MRT_SortingState
} from 'material-react-table';
import MUITableCommonOptions from './../common/MUITableCommonOptions';
import { Box, Tooltip, IconButton, Button, Toolbar } from '@mui/material';
import IDtResult from './interfaces/datatables/IDtResult';
import IDtFetchData from './interfaces/datatables/IDtFetchData';
import IVehicleListItem from './interfaces/IVehicleListItem';
import IDtParams from './interfaces/datatables/IDtParams';
import DetailIcon from '@mui/icons-material/VisibilityOutlined';

interface IAdvancedFeatures {
  enableSorting: boolean;
  enablePagination: boolean;
  enableTopToolbar: boolean;
}

export const DriverDatatable: React.FC<{
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>,
  autoFetch: boolean,
}> = ({ fetchDataRef, autoFetch }) => {
  const navigate = useNavigate();

  // Data and fetching state
  const [data, setData] = useState<IVehicleListItem[]>([]);
  const [isError, setIsError] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [isRefetching, setIsRefetching] = useState(false);
  const [rowCount, setRowCount] = useState(0);

  // Table state
  const [columnFilters, setColumnFilters] = useState<MRT_ColumnFiltersState>([]);
  const [globalFilter, setGlobalFilter] = useState('');
  const [sorting, setSorting] = useState<MRT_SortingState>([]);
  const [pagination, setPagination] = useState<MRT_PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  });

  const disableFeaturesObj: IAdvancedFeatures = {
    enableSorting: false,
    enablePagination: false,
    enableTopToolbar: false,
  };
  const enableFeaturesObj: IAdvancedFeatures = {
    enableSorting: true,
    enablePagination: true,
    enableTopToolbar: true,
  };
  const [enableAdvancedFeatures, setEnableAdvancedFeatures] = useState<IAdvancedFeatures>(
    autoFetch ? enableFeaturesObj : disableFeaturesObj);

  const fetchData = async () => {
    //if (!data.length) {
    //  setIsLoading(true);
    //}
    //else {
    //  setIsRefetching(true);
    //}

    const startOffset = pagination.pageIndex * pagination.pageSize;
    let dtParams: IDtParams = {
      start: startOffset,
      size: pagination.pageSize,
      filters: columnFilters ?? [],
      sorting: sorting ?? [],
    };
    try {
      const response = await fetch(`/api/PersonSearch`, {
        //const response = await fetch(`/api/PersonSearch`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(dtParams)
      });
      console.log(response);
      //const json: IDtResult<IVehicleListItem> = await response.json();
      //if (json.data.length === 1)
      //  navigate(`/Person/${json.data[0].id}`);
      //setEnableAdvancedFeatures(enableFeaturesObj);

      //setData(json.data);
      //setRowCount(json.totalRowCount);
    }
    catch (error) {
      setIsError(true);
      console.error(error);
      return;
    }
    //setIsError(false);
    //setIsLoading(false);
    //setIsRefetching(false);

    // Show table body
    //const tableBody = document.querySelector('tbody') as HTMLElement;
    //tableBody.style.display = 'table-row-group';
  };
  fetchDataRef.current = fetchData;

  fetchData();

  //useEffect(() => {
  //  if (autoFetch || enableAdvancedFeatures.enableSorting)
  //    fetchDataRef.current?.();
  //}, [
  //  pagination.pageIndex,
  //  pagination.pageSize,
  //  sorting,
  //]);

  //const columns = useMemo<MRT_ColumnDef<IVehicleListItem>[]>(
  //  () => [
  //    //{
  //    //  id: 'vin',
  //    //  accessorKey: 'vin',
  //    //  header: 'VIN',
  //    //  filterFn: 'startsWith',
  //    //},
  //    //{
  //    //  id: 'licensePlate',
  //    //  accessorKey: 'licensePlate',
  //    //  header: 'SPZ',
  //    //  filterFn: 'startsWith',
  //    //},
  //    //{
  //    //  id: 'ownerFullName',
  //    //  accessorKey: 'ownerFullName',
  //    //  header: 'Vlastník',
  //    //  filterFn: 'startsWith',
  //    //},
  //    //{
  //    //  id: 'manufacturer',
  //    //  accessorKey: 'manufacturer',
  //    //  header: 'Výrobce',
  //    //  filterFn: 'startsWith',
  //    //},
  //    //{
  //    //  id: 'model',
  //    //  accessorKey: 'model',
  //    //  header: 'Model',
  //    //  filterFn: 'startsWith',
  //    //},
  //    //{
  //    //  id: 'color',
  //    //  accessorKey: 'color',
  //    //  header: 'Barva',
  //    //  filterFn: 'startsWith',
  //    //},
  //    //{
  //    //  id: 'manufacturedYear',
  //    //  accessorKey: 'manufacturedYear',
  //    //  header: 'Rok výroby',
  //    //  filterFn: 'startsWith',
  //    //},


  //    {
  //      id: 'firstName',
  //      accessorKey: 'firstName',
  //      header: 'Křestní jméno',
  //      filterFn: 'startsWith',
  //    },
  //    {
  //      id: 'lastName',
  //      accessorKey: 'lastName',
  //      header: 'Příjmení',
  //      filterFn: 'startsWith',
  //    },
  //    {
  //      id: 'birthNumber',
  //      accessorKey: 'birthNumber',
  //      header: 'Rodné číslo',
  //      filterFn: 'startsWith',
  //    },
  //    {
  //      id: 'driversLicenseNumber',
  //      accessorKey: 'driversLicenseNumber',
  //      header: 'driversLicenseNumber',
  //      filterFn: 'Řidický průkaz',
  //    },
  //  ],
  //  []
  //);

  //// todo rework
  //// todo probably should perform on component mount
  //useEffect(() => {
  //  const tableBody = document.querySelector('tbody') as HTMLElement;
  //  tableBody.style.display = 'none';
  //}, []);

  //const table = useMaterialReactTable({
  //  ...MUITableCommonOptions<IVehicleListItem>(), // Add common and basic options
  //  columns,
  //  data,
  //  onColumnFiltersChange: setColumnFilters,
  //  onGlobalFilterChange: setGlobalFilter,
  //  onPaginationChange: setPagination,
  //  onSortingChange: setSorting,
  //  rowCount,
  //  state: {
  //    columnFilters,
  //    globalFilter,
  //    isLoading,
  //    pagination,
  //    showAlertBanner: isError,
  //    showProgressBars: isRefetching,
  //    sorting,
  //  },
  //  ...enableAdvancedFeatures,
  //  enableRowActions: true,       // Display row actions
  //  renderRowActions: ({ row }) => (
  //    <Box sx={{ display: 'flex', gap: '1rem' }}>
  //      <Tooltip title="Zobrazit detail vozidla">
  //        <IconButton onClick={() => navigate(`/vehicle/${row.original.id}`)}>
  //          <DetailIcon />
  //        </IconButton>
  //      </Tooltip>
  //    </Box>
  //  ),
  //});

  return (
    <>
      {enableAdvancedFeatures.enableTopToolbar && (
        <Toolbar>
          <Button variant="contained" color="primary" onClick={() => fetchDataRef.current?.()}>
            Vyhledat
          </Button>
        </Toolbar>
      )}
      
    </>
  );
};
//<MaterialReactTable table={table} />
export default DriverDatatable;
