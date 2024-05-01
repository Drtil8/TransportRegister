import { useEffect, useMemo, useState } from 'react';
import IUserListItem from "../interfaces/IUserListItem";
import {
  MaterialReactTable, useMaterialReactTable,
  type MRT_ColumnDef, type MRT_ColumnFiltersState,
  type MRT_PaginationState, type MRT_SortingState
} from 'material-react-table';
import MUITableCommonOptions from './../../common/MUITableCommonOptions';
import { Box, Tooltip, IconButton } from '@mui/material';
import IDtResult from '../interfaces/datatables/IDtResult';
import IDtFetchData from '../interfaces/datatables/IDtFetchData';
import IDtParams from '../interfaces/datatables/IDtParams';
import DeleteIcon from '@mui/icons-material/Delete';
import UserDeleteModal from './UserDeleteModal';
import RestoreIcon from '@mui/icons-material/Restore';

export const UserDatatable: React.FC<{
  fetchDataRef: React.MutableRefObject<IDtFetchData | null>
}> = ({ fetchDataRef }) => {
  const [data, setData] = useState<IUserListItem[]>([]);
  const [isError, setIsError] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [isRefetching, setIsRefetching] = useState(false);
  const [rowCount, setRowCount] = useState(0);

  // Modals
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [userId, setUserId] = useState<string | null>(null);

  const openDeleteModal = (userId: string) => {
    setUserId(userId);
    setIsDeleteModalOpen(true);
  }

  const toggleDeleteModal = () => {
    setIsDeleteModalOpen(!isDeleteModalOpen);
  }

  const restoreUser = async (userId: string) => {
    try {
      const response = await fetch(`/api/User/${userId}/Restore`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        }
      });
      if (response.ok) {
        fetchDataRef.current?.();
      }
      else {
        setIsError(true);
      }
    }
    catch (error) {
      setIsError(true);
      console.error(error);
    }
  }

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
      const response = await fetch(`/api/Users`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(dtParams)
      });
      const json: IDtResult<IUserListItem> = await response.json();
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

  const columns = useMemo<MRT_ColumnDef<IUserListItem>[]>(
    () => [
      //{
      //  id: 'id',
      //  accessorKey: 'id',
      //  header: 'ID',
      //  filterFn: 'startsWith',
      //},
      {
        id: 'fullName',
        accessorKey: 'fullName',
        header: 'Jméno',
        filterFn: 'startsWith',
      },
      {
        id: 'role',
        accessorKey: 'role',
        header: 'Role',
        filterFn: 'startsWith',
      },
      {
        id: 'email',
        accessorKey: 'email',
        header: 'Email',
        //filterVariant: 'select',
        //filterSelectOptions: ['Car', 'Motorcycle', 'Truck', 'Bus'],
      }
    ],
    []
  );

  useEffect(() => {
    fetchDataRef.current?.();
  }, [
    columnFilters,
    globalFilter,
    pagination.pageIndex,
    pagination.pageSize,
    sorting,
  ]);

  const table = useMaterialReactTable({
    ...MUITableCommonOptions<IUserListItem>(), // Add common and basic options
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
    //enableSorting: false,   // todo create state to enable sorting dynamically
    //enablePagination: false,
    //enableTopToolbar: false,
    enableRowActions: true,       // Display row actions
    renderRowActions: ({ row }) => (// todo why not href instead of onClick
      <Box sx={{ display: 'flex', gap: '1rem' }}>
        {row.original.isValid ? (
          <Tooltip title="Smazat uživatele">
            <IconButton color="error" onClick={() => openDeleteModal(row.original.id) }>
              <DeleteIcon />
            </IconButton>
          </Tooltip>
        )
          :
          (
            <Tooltip title="Obnovit uživatele">
              <IconButton color="success" onClick={() => restoreUser(row.original.id) }>
                <RestoreIcon />
              </IconButton>
            </Tooltip>
          )}
        {/*TODO vacation icon*/}
      </Box>
    ),
    muiTableBodyRowProps: (table) => ({
      className: (table.row.original.isValid) ? '' : "invalid-item",
    }),
  });

  return (
    <>
      <UserDeleteModal userId={userId} isOpen={isDeleteModalOpen} toggle={toggleDeleteModal} fetchData={fetchData}  />
      <MaterialReactTable table={table} />
    </>
  );
}

export default UserDatatable;
