import { useEffect, useMemo, useState } from 'react';
import {
  MaterialReactTable, useMaterialReactTable,
  type MRT_ColumnDef, type MRT_ColumnFiltersState,
  type MRT_PaginationState, type MRT_SortingState
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
  const initialColumnSort: ColumnSort = { id: 'start', desc: true };
  const [columnFilters, setColumnFilters] = useState<MRT_ColumnFiltersState>([]);
  const [globalFilter, setGlobalFilter] = useState('');
  const [sorting, setSorting] = useState<MRT_SortingState>([initialColumnSort]);
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
    };
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
  ]);


  const columns = useMemo<MRT_ColumnDef<ITheftListItem>[]>(
    () => [
      {
        id: 'theftId',
        accessorKey: 'theftId',
        header: 'T.ID',
      },
      {
        id: 'reportedOn',
        accessorFn: (row) => new Date(row.reportedOn),
        header: 'Nahlášeno',
        /*filterVariant: 'date',*/
        //filterFn: 'greaterThan',
        //sortingFn: 'datetime',
        Cell: ({ cell }) => formatDate(cell.getValue<Date>()),
      },
      {
        id: 'stolenOn',
        accessorFn: (row) => new Date(row.stolenOn),
        header: 'Ukradeno',
        /*filterVariant: 'date',*/
        //filterFn: 'greaterThan',
        //sortingFn: 'datetime',
        Cell: ({ cell }) => formatDate(cell.getValue<Date>()),
      },
      {
        id: 'foundOn',
        accessorFn: (row) => row.foundOn !== null ? new Date(row.foundOn) : null,
        header: 'Nalezeno',
        /*filterVariant: 'date',*/
        //filterFn: 'greaterThan',
        //sortingFn: 'datetime',
        Cell: ({ cell }) => cell.getValue<Date>() !== null ? formatDate(cell!.getValue<Date>()) : 'Nenalezeno',
      },
      {
        id: 'vehiclePlate', // Use a unique id for the column
        accessorFn: (row) => row.vehicle?.licensePlate ?? 'N/A', // Access the plate property of the vehicle object
        header: 'SPZ',
      },
      {
        id: 'vehicleManufacturer', // Use a unique id for the column
        accessorFn: (row) => row.vehicle?.manufacturer ?? 'N/A', // Access the plate property of the vehicle object
        header: 'Výrobce',
      },
      {
        id: 'vehicleModel', // Use a unique id for the column
        accessorFn: (row) => row.vehicle?.model ?? 'N/A', // Access the plate property of the vehicle object
        header: 'Model',
      },
    ],
    []
  );

  const goTo = (theftId: number) => {
    navigate(`/theft/${theftId}`);
  }

  const table = useMaterialReactTable({
    ...MUITableCommonOptions<ITheftListItem>(), // Add common and basic options
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
      className: (!table.row.original.isFound ? 'invalid-item' : (table.row.original.isReturned ? 'valid-item' : "workedOn-item")),
    }),
  });

  return (
    <>
      <MaterialReactTable table={table} />
    </>
  );
};

export default TheftDatatable;
