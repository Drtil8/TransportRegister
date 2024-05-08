import { useEffect, useMemo, useState } from 'react';
import {
  MaterialReactTable, useMaterialReactTable,
  type MRT_ColumnDef, type MRT_ColumnFiltersState,
  type MRT_PaginationState, type MRT_SortingState, type MRT_ColumnFilterFnsState
} from 'material-react-table';
import { Box, Tooltip, IconButton } from '@mui/material';
import { ColumnSort } from '@tanstack/react-table';
import MUITableCommonOptions from '../../common/MUITableCommonOptions';
import { formatDate } from '../../common/DateFormatter';
import IDtResult from '../interfaces/datatables/IDtResult';
import IDtFetchData from '../interfaces/datatables/IDtFetchData';
import IDtParams from '../interfaces/datatables/IDtParams';
import ITheftListItem from '../interfaces/ITheftListItem';
import DetailIcon from '@mui/icons-material/VisibilityOutlined';
import { useNavigate } from 'react-router-dom';

export const TheftDatatable: React.FC<{
  fetchUrl: string,
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>,
}> = ({ fetchUrl, fetchDataRef }) => {
  const navigate = useNavigate();

  // Data and fetching state
  const [data, setData] = useState<ITheftListItem[]>([]);
  const [isError, setIsError] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [isRefetching, setIsRefetching] = useState(false);
  const [rowCount, setRowCount] = useState(0);

  // Table state
  const initialFilterOptions: MRT_ColumnFilterFnsState = {
    reportedOn: 'equals',
    stolenOn: 'equals',
    foundOn: 'equals'
  };
  const dateFilterOptions = ['equals', 'lessThan', 'greaterThan'];    // try 'between'
  const [columnFilterOptions, setColumnFilterOptions] = useState<MRT_ColumnFilterFnsState>(initialFilterOptions);
  const [columnFilters, setColumnFilters] = useState<MRT_ColumnFiltersState>([]);

  const initialColumnSort: ColumnSort = { id: 'theftId', desc: true };
  const [sorting, setSorting] = useState<MRT_SortingState>([initialColumnSort]);

  const [globalFilter, setGlobalFilter] = useState('');
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
    let searchParams: IDtParams = {
      start: startOffset,
      size: pagination.pageSize,
      filters: columnFilters ?? [],
      sorting: sorting ?? [],
      filterOptions: Object.entries(columnFilterOptions).map(([id, option]) => ({ id, option })),
    };
    console.log(searchParams);

    try {
      const response = await fetch(fetchUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(searchParams)
      });
      const json: IDtResult<ITheftListItem> = await response.json();
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
  };
  fetchDataRef.current = fetchData;

  useEffect(() => {
    fetchDataRef.current?.();
  }, [
    columnFilters,
    globalFilter,
    pagination.pageIndex,
    pagination.pageSize,
    sorting,
    columnFilterOptions
  ]);

  const columns = useMemo<MRT_ColumnDef<ITheftListItem>[]>(
    () => [
      {
        id: 'theftId',
        accessorKey: 'theftId',
        header: 'T.ID',
        enableColumnFilterModes: false,
      },
      {
        id: 'reportedOn',
        accessorFn: (row) => new Date(row.reportedOn),
        header: 'Nahlášeno',
        filterVariant: 'date',
        sortingFn: 'datetime',
        columnFilterModeOptions: dateFilterOptions,
        Cell: ({ cell }) => formatDate(cell.getValue<Date>()),
      },
      {
        id: 'stolenOn',
        accessorFn: (row) => new Date(row.stolenOn),
        header: 'Ukradeno',
        filterVariant: 'date',
        sortingFn: 'datetime',
        columnFilterModeOptions: dateFilterOptions,
        Cell: ({ cell }) => formatDate(cell.getValue<Date>()),
      },
      {
        id: 'foundOn',
        accessorFn: (row) => row.foundOn !== null ? new Date(row.foundOn) : null,
        header: 'Nalezeno',
        filterVariant: 'date',
        sortingFn: 'datetime',
        columnFilterModeOptions: dateFilterOptions,
        Cell: ({ cell }) => cell.getValue<Date>() !== null ? formatDate(cell!.getValue<Date>()) : 'Nenalezeno',
      },
      {
        id: 'Vehicle.LicensePlate', // Use a unique id for the column
        accessorFn: (row) => row.vehicle?.licensePlate ?? 'N/A',
        header: 'SPZ',
        enableColumnFilterModes: false,
      },
      {
        id: 'Vehicle.Manufacturer',
        accessorFn: (row) => row.vehicle?.manufacturer ?? 'N/A',
        header: 'Výrobce',
        enableColumnFilterModes: false,
      },
      {
        id: 'Vehicle.Model',
        accessorFn: (row) => row.vehicle?.model ?? 'N/A',
        header: 'Model',
        enableColumnFilterModes: false,
      },
    ],
    []
  );

  const goTo = (theftId: number) => {
    navigate(`/theft/${theftId}`);
  }

  const table = useMaterialReactTable({
    ...MUITableCommonOptions<ITheftListItem>(), // Add common and basic options
    enableColumnFilterModes: true,
    columns,
    data,
    onColumnFiltersChange: setColumnFilters,
    onGlobalFilterChange: setGlobalFilter,
    onPaginationChange: setPagination,
    onSortingChange: setSorting,
    onColumnFilterFnsChange: setColumnFilterOptions,
    rowCount,
    state: {
      columnFilters,
      globalFilter,
      isLoading,
      pagination,
      showAlertBanner: isError,
      showProgressBars: isRefetching,
      sorting,
      columnFilterFns: columnFilterOptions,
    },
    enableRowActions: true,        // Display row actions
    renderRowActions: ({ row }) =>
    (
      <Box sx={{ display: 'flex', gap: '1rem' }}>
        <Tooltip title="Zobrazit detail krádeže">
          <IconButton onClick={() => goTo(row.original.theftId)}>
            <DetailIcon />
          </IconButton>
        </Tooltip>
      </Box>
    ),
    muiTableBodyRowProps: (table) => ({
      className: (!table.row.original.isFound ?
        'invalid-item'
        :
        (table.row.original.isReturned ? 'valid-item' : "workedOn-item")),
    }),
  });

  return (
    <MaterialReactTable table={table} />
  );
};

export default TheftDatatable;
