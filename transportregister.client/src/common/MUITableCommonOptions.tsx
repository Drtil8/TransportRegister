﻿import { MRT_RowData, MRT_TableOptions } from "material-react-table";
import { MRT_Localization_CS } from "material-react-table/locales/cs";

function MUITableCommonOptions<T extends MRT_RowData>(): MRT_TableOptions<T> {
  return {
    columns: [],  // required option, but will be overwritten
    data: [],     // required option, but will be overwritten
    enableFilters: true,
    enableTopToolbar: true,
    //enableColumnResizing: true,
    enableRowSelection: false,
    enableColumnFilterModes: false,
    enableGlobalFilter: false,
    positionActionsColumn: 'last',
    manualFiltering: true,
    manualPagination: true,
    manualSorting: true,
    initialState: {
      showColumnFilters: true,
      showGlobalFilter: false,
      columnPinning: {
        right: ['mrt-row-actions'],
      },
      density: 'compact',
    },
    localization: MRT_Localization_CS,
    paginationDisplayMode: 'pages',
    positionToolbarAlertBanner: 'bottom',
    muiSearchTextFieldProps: {
      size: 'small',
      variant: 'outlined',
    },
    muiPaginationProps: {
      color: 'secondary',
      rowsPerPageOptions: [5, 7, 10, 12, 15, 20, 25, 50, 100],
      shape: 'rounded',
      variant: 'outlined',
    },
  };
}

export default MUITableCommonOptions;
