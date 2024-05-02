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
import { Box, Tooltip, IconButton } from '@mui/material';
import IDtResult from './interfaces/datatables/IDtResult';
import IDtFetchData from './interfaces/datatables/IDtFetchData';
import IDtParams from './interfaces/datatables/IDtParams';
import DetailIcon from '@mui/icons-material/VisibilityOutlined';
import IDriverSimpleList from './interfaces/IDriverSimpleList';

export const DriverDatatable: React.FC<{
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>,
  autoFetch: boolean,
}> = ({ fetchDataRef, autoFetch }) => {
  const navigate = useNavigate();

  // Data and fetching state
  const [data, setData] = useState<IDriverSimpleList[]>([]);
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

  const fetchData = async () => {
    if (!data.length) {
      setIsLoading(true);
    }
    else {
      setIsRefetching(true);
    }

    const startOffset = pagination.pageIndex * pagination.pageSize;
    let dtParams: IDtParams = {
      start: startOffset,
      size: pagination.pageSize,
      filters: columnFilters ?? [],
      sorting: sorting ?? [],
    };
    try {
      const response = await fetch(`/api/PersonSearch`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(dtParams)
      });
      if (response.ok) {
        const json: IDtResult<IDriverSimpleList> = await response.json();
        //if (json.data.length === 1)
        //  navigate(`/Person/${json.data[0].id}`);
        setData(json.data);
        setRowCount(json.totalRowCount);
      }
    }
    catch (error) {
      setIsError(true);
      console.error(error);
      return;
    }
    setIsError(false);
    setIsLoading(false);
    setIsRefetching(false);

    // Show table body
    const tableBody = document.querySelector('tbody') as HTMLElement;
    tableBody.style.display = 'table-row-group';
  };
  fetchDataRef.current = fetchData;

  useEffect(() => {
    if (autoFetch)
      fetchDataRef.current?.();
  }, [
    columnFilters,
    pagination.pageIndex,
    pagination.pageSize,
    sorting,
  ]);

  const columns = useMemo<MRT_ColumnDef<IDriverSimpleList>[]>(
    () => [
      {
        id: 'firstName',
        accessorKey: 'firstName',
        header: 'Jméno',
        filterFn: 'startsWith',
      },
      {
        id: 'lastName',
        accessorKey: 'lastName',
        header: 'Příjmení',
        filterFn: 'startsWith',
      },
      {
        id: 'birthNumber',
        accessorKey: 'birthNumber',
        header: 'Rodné číslo',
        filterFn: 'startsWith',
      },
      {
        id: 'driversLicenseNumber',
        accessorKey: 'driversLicenseNumber',
        header: 'Řidický průkaz',
        filterFn: 'startsWith',
      },
    ],
    []
  );

  // todo rework
  // todo probably should perform on component mount
  useEffect(() => {
    const tableBody = document.querySelector('tbody') as HTMLElement;
    tableBody.style.display = 'none';
  }, []);

  const table = useMaterialReactTable({
    ...MUITableCommonOptions<IDriverSimpleList>(), // Add common and basic options
    columns,
    data,
    onColumnFiltersChange: setColumnFilters,
    onGlobalFilterChange: setGlobalFilter,
    onPaginationChange: setPagination,
    onSortingChange: setSorting,
    rowCount,
    state: {
      columnFilters,
      globalFilter,
      isLoading,
      pagination,
      showAlertBanner: isError,
      showProgressBars: isRefetching,
      sorting,
    },
    enableSorting: true,
    enablePagination: true,
    enableTopToolbar: false,
    enableRowActions: true,       // Display row actions
    renderRowActions: ({ row }) => (
      <Box sx={{ display: 'flex', gap: '1rem' }}>
        <Tooltip title="Zobrazit detail">
          <IconButton onClick={() => navigate(`/driver/${row.original.personId}`)}>
            <DetailIcon />
          </IconButton>
        </Tooltip>
      </Box>
    ),
  });

  // todo add search button
  //<Toolbar>
  //  <Button variant="contained" color="primary" onClick={() => fetchDataRef.current?.()}>
  //    Vyhledat
  //  </Button>
  //</Toolbar>
  return (
      <MaterialReactTable table={table} />
  );
};
export default DriverDatatable;
