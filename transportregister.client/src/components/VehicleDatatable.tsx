import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  MaterialReactTable, useMaterialReactTable,
  type MRT_ColumnDef, type MRT_ColumnFiltersState,
  type MRT_PaginationState, type MRT_SortingState
} from 'material-react-table';
import MUITableCommonOptions from './../common/MUITableCommonOptions';
import { Box, Tooltip, IconButton } from '@mui/material';
import IDtResult from './interfaces/datatables/IDtResult';
import IDtFetchData from './interfaces/datatables/IDtFetchData';
import IVehicleListItem from './interfaces/IVehicleListItem';
import IDtParams from './interfaces/datatables/IDtParams';
import DetailIcon from '@mui/icons-material/VisibilityOutlined';
import { Button } from 'reactstrap';

export const VehicleDatatable: React.FC<{
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>
}> = ({ fetchDataRef }) => {
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
      const response = await fetch(`/api/VehicleSearch`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(dtParams)
      });
      const json: IDtResult<IVehicleListItem> = await response.json();
      //console.log(json.data);     // todo fix invalid array format with c# JsonConvertors
      setData(json.data);
      setRowCount(json.totalRowCount);
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

  //useEffect(() => {
  //  fetchDataRef.current?.();
  //}, [
  //  columnFilters,
  //  globalFilter,
  //  pagination.pageIndex,
  //  pagination.pageSize,
  //  sorting,
  //]);

  const columns = useMemo<MRT_ColumnDef<IVehicleListItem>[]>(
    () => [
      {
        id: 'vin',
        accessorKey: 'vin',
        header: 'VIN',
        filterFn: 'startsWith',
      },
      {
        id: 'licensePlate',
        accessorKey: 'licensePlate',
        header: 'SPZ',
        filterFn: 'startsWith',
      },
      {
        id: 'ownerFullName',
        accessorKey: 'ownerFullName',
        header: 'Vlastník',
        filterFn: 'startsWith',
      },
      {
        id: 'vehicleType',
        accessorKey: 'vehicleType',
        header: 'Typ vozidla',
        filterVariant: 'select',
        filterSelectOptions: ['Car', 'Motorcycle', 'Truck', 'Bus'],
      },
      {
        id: 'manufacturer',
        accessorKey: 'manufacturer',
        header: 'Výrobce',
        filterFn: 'startsWith',
      },
      {
        id: 'model',
        accessorKey: 'model',
        header: 'Model',
        filterFn: 'startsWith',
      },
      {
        id: 'color',
        accessorKey: 'color',
        header: 'Barva',
        filterFn: 'startsWith',
      },
      {
        id: 'manufacturedYear',
        accessorKey: 'manufacturedYear',
        header: 'Rok výroby',
        filterFn: 'startsWith',
      },
    ],
    []
  );

  // todo probably should perform on component mount
  useEffect(() => {
    const actionTableHeader = document.querySelector('th:last-child') as HTMLElement;
    const existingButton = actionTableHeader.querySelector('button');
    if (!existingButton) {
      let searchButton = document.createElement('button');
      searchButton.classList.add('btn');
      searchButton.classList.add('btn-primary');
      searchButton.textContent = 'Vyhledat';
      searchButton.addEventListener('click', () => { fetchDataRef.current?.() });
      actionTableHeader.appendChild(searchButton);
    }
    const tableBody = document.querySelector('tbody') as HTMLElement;
    tableBody.style.display = 'none';
  }, []);

  const table = useMaterialReactTable({
    ...MUITableCommonOptions<IVehicleListItem>(), // Add common and basic options
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
    enableSorting: false,   // todo create state to enable sorting dynamically
    enablePagination: false,
    enableTopToolbar: false,
    enableRowActions: true,       // Display row actions
    renderRowActions: ({ row }) => (// todo why not href instead of onClick
      <Box sx={{ display: 'flex', gap: '1rem' }}>
        <Tooltip title="Zobrazit detail vozidla">
          <IconButton onClick={() => navigate(`/vehicle/${row.original.id}`)}>
            <DetailIcon />
          </IconButton>
        </Tooltip>
      </Box>
    ),
  });

  return (
    <MaterialReactTable table={table} />
  );
};

export default VehicleDatatable;
