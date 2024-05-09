import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { createRoot } from 'react-dom/client';
import {
  MRT_RowSelectionState,
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
import DtSearchButton from './DtSearchButton';

interface IAdvancedFeatures {
  enableSorting: boolean;
  enablePagination: boolean;
}

export const DriverDatatable: React.FC<{
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>,
  autoFetch: boolean,
  selectable: boolean,
  ownerId?: number | null,
}> = ({ fetchDataRef, autoFetch, selectable, ownerId }) => {
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

  const disableFeaturesObj: IAdvancedFeatures = {
    enableSorting: false,
    enablePagination: false,
  };
  const enableFeaturesObj: IAdvancedFeatures = {
    enableSorting: true,
    enablePagination: true,
  };
  const [enableAdvancedFeatures, setEnableAdvancedFeatures] = useState<IAdvancedFeatures>(
    autoFetch || ownerId ? enableFeaturesObj : disableFeaturesObj);

  const [rowSelection, setRowSelection] = useState<MRT_RowSelectionState>({});

  const fetchData = async () => {
    if (!data.length) {
      setIsLoading(true);
    }
    else {
      setIsRefetching(true);
    }

    if (clickedOnSearch)
      deletePersonIdFromColumnFilters();

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
        if (json.data.length === 1 && !selectable)
          navigate(`/driver/${json.data[0].personId}`);   // todo only after onclick search
        setEnableAdvancedFeatures(enableFeaturesObj);
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
    if (autoFetch || enableAdvancedFeatures.enableSorting)
      fetchDataRef.current?.();
  }, [
    pagination.pageIndex,
    pagination.pageSize,
    sorting,
  ]);

  const columns = useMemo<MRT_ColumnDef<IDriverSimpleList>[]>(
    () => [
      {
        id: 'driversLicenseNumber',
        accessorKey: 'driversLicenseNumber',
        header: 'Řidický průkaz',
        filterFn: 'startsWith',
      },
      {
        id: 'birthNumber',
        accessorKey: 'birthNumber',
        header: 'Rodné číslo',
        filterFn: 'startsWith',
      },
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
    ],
    []
  );

  const deletePersonIdFromColumnFilters = () => {
    let updatedFilters = columnFilters.filter(filter => filter.id !== 'personId');
    setColumnFilters(updatedFilters);
  }

  const [clickedOnSearch, setClickedOnSearch] = useState<boolean>(false);

  const onSearchClick = () => {
    setClickedOnSearch(true);
  }

  useEffect(() => {
    if (clickedOnSearch) {
      setClickedOnSearch(false);
      fetchDataRef.current?.();
    }
  }, [clickedOnSearch, columnFilters]);


  const renderSearchButton = () => {
    const actionTableHeader = document.querySelector('th:last-child') as HTMLElement;
    const existingButton = actionTableHeader.querySelector('button');
    if (!existingButton) {
      // Add search button
      const buttonContainer = document.createElement('div');
      actionTableHeader.appendChild(buttonContainer);
      const root = createRoot(buttonContainer);
      root.render(<DtSearchButton onSearchClick={onSearchClick} />);
    }
    const tableBody = document.querySelector('tbody') as HTMLElement;
    tableBody.style.display = 'none';
  }

  useEffect(() => {
    renderSearchButton();
  }, []);

  useEffect(() => {
    const clearButton = document.querySelector('.MuiAlert-message') as HTMLElement;
    if (clearButton)
      clearButton.style.display = 'none';

    let selectedRows = Object.keys(rowSelection);
    if (selectedRows.length === 1) {
      let selectedOwnerId = selectedRows[0];
      let inputOwnerId = (document.querySelector('input[name="ownerId"]') as HTMLInputElement);
      inputOwnerId.value = selectedOwnerId;

      let drivingLicense = data.find((d) => d.personId === parseInt(selectedOwnerId))?.driversLicenseNumber ?? null;
      let birthNumber = data.find((d) => d.personId === parseInt(selectedOwnerId))?.birthNumber ?? null;
      let newColumnFilters = columnFilters.filter(filter =>
        filter.id !== 'driversLicenseNumber'
        && filter.id !== 'birthNumber'
        && filter.id !== 'personId');
      if (drivingLicense !== null)
        newColumnFilters.push({ id: 'driversLicenseNumber', value: drivingLicense });
      if (birthNumber !== null)
        newColumnFilters.push({ id: 'birthNumber', value: birthNumber });
      if (selectedOwnerId)
        newColumnFilters.push({ id: 'personId', value: selectedOwnerId });
      setColumnFilters(newColumnFilters);

      const timer = setTimeout(() => {
        fetchDataRef.current?.();
      }, 200);
      return () => clearTimeout(timer);
    }
  }, [rowSelection]);

  useEffect(() => {
    if (ownerId) {
      setRowSelection({ [ownerId]: true });
    }
  }, []);

  const table = useMaterialReactTable({
    ...MUITableCommonOptions<IDriverSimpleList>(), // Add common and basic options
    columns,
    data,
    getRowId: (row) => String(row.personId),
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
      rowSelection,
    },
    ...enableAdvancedFeatures,
    enableTopToolbar: false,
    enableRowSelection: selectable, // Enable row selection
    onRowSelectionChange: setRowSelection,
    enableSelectAll: false,
    enableMultiRowSelection: false,
    enableRowActions: true,  // Display row actions
    renderRowActions: ({ row }) => (
      <Box sx={{ display: 'flex', gap: '1rem' }}>
        {!selectable && (
          <Tooltip title="Zobrazit detail">
            <IconButton onClick={() => navigate(`/driver/${row.original.personId}`)}>
              <DetailIcon />
            </IconButton>
          </Tooltip>
        )}
      </Box>
    ),
  });

  return (
    <MaterialReactTable table={table} />
  );
};
export default DriverDatatable;
