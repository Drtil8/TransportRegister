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
import IOffenceListItem from '../interfaces/IOffenceListItem';
import DetailIcon from '@mui/icons-material/VisibilityOutlined';
//import AuthContext from '../../auth/AuthContext';
import { useNavigate } from 'react-router-dom';

export const OffenceDatatable: React.FC<{
  fetchUrl: string,
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>,
}> = ({ fetchUrl, fetchDataRef }) => {
  //const authContext = useContext(AuthContext);
  const navigate = useNavigate();

  // Data and fetching state
  const [data, setData] = useState<IOffenceListItem[]>([]);
  const [isError, setIsError] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [isRefetching, setIsRefetching] = useState(false);
  const [rowCount, setRowCount] = useState(0);

  // Table state
  const initialColumnSort: ColumnSort = { id: 'offenceId', desc: true };
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
      const json: IDtResult<IOffenceListItem> = await response.json();
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

  const columns = useMemo<MRT_ColumnDef<IOffenceListItem>[]>(
    () => [
      {
        id: 'offenceId',
        accessorKey: 'offenceId',
        header: 'P.ID',
      },
      {
        id: 'offenceType',
        accessorKey: 'offenceType',
        header: 'Typ přestupku',
      },
      {
        id: 'reportedOn',
        accessorFn: (row) => new Date(row.reportedOn),
        header: 'Spáchán',
        /*filterVariant: 'date',*/
        //filterFn: 'greaterThan',
        //sortingFn: 'datetime',
        Cell: ({ cell }) => formatDate(cell.getValue<Date>()),
      },
      {
        id: 'penaltyPoints',
        accessorKey: 'penaltyPoints',
        header: 'Trestné body',
      },
      {
        id: 'Person.FullName', // Use a unique id for the column
        accessorFn: (row) => row.person.fullName, // Access the fullName property of the person object
        header: 'Osoba',
      },
      {
        id: 'Person.BirthNumber',
        accessorFn: (row) => row.person.birthNumber, // Access the fullName property of the person object
        header: 'Rodné číslo',
      },
      {
        id: 'Vehicle.LicensePlate', // Use a unique id for the column
        accessorFn: (row) => row.vehicle?.licensePlate ?? 'N/A', // Access the plate property of the vehicle object
        header: 'SPZ',
      },
      // TODO
    ],
    []
  );

  const goTo = (offenceId: number) => {
    navigate(`/offence/${offenceId}`);
  }

  const table = useMaterialReactTable({
    ...MUITableCommonOptions<IOffenceListItem>(), // Add common and basic options
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
        <Tooltip title="Zobrazit detail přestupku">
          <IconButton onClick={() => goTo(row.original.offenceId)}>
            <DetailIcon />
          </IconButton>
        </Tooltip>
      </Box>
    ),
    muiTableBodyRowProps: (table) => ({
      className: (table.row.original.isValid && table.row.original.isApproved) ? 'valid-item' : (table.row.original.isValid && !table.row.original.isApproved ? "workedOn-item" : "invalid-item"),
    }),
  });

  return (
    <>
      <MaterialReactTable table={table} />
    </>
  );
};

export default OffenceDatatable;
