﻿import { VehicleList } from "./components/VehicleList";
import Login from './components/Login';
import VehicleSearch from "./components/VehicleSearch";
import DriverSearch from "./components/DriverSearch";
import DriverCreate from "./components/DriverCreate";
import VehicleCreate from "./components/VehicleCreate";
import OffenceCreate from "./components/OffenceCreate";
import OffencePending from "./components/OffencePending";
import TheftCreate from "./components/TheftCreate";
import TheftFound from "./components/TheftFound";
import DriverDetail from "./components/DriverDetail";
import VehicleDetail from "./components/VehicleDetail";

const AppRoutes = [
  {
    path: '/',
    element: <VehicleList />,
    isProtected: true
  },
  {
    path: '/login',
    element: <Login />,
    isProtected: false
  },
  {
    path: '/driverSearch',
    element: <DriverSearch />,
    isProtected: true
  },
  {
    path: '/driverCreate',
    element: <DriverCreate />,
    isProtected: true
  },
  {
    path: '/driver/:id',   // TODO delete, DrierDetail will not be called like this, this is a bad solution for GUI example
    element: <DriverDetail />,
    isProtected: true
  },
  {
    path: '/vehicle/:id',
    element: <VehicleDetail />,
    isProtected: true
  },
  {
    path: '/vehicleSearch',
    element: <VehicleSearch />,
    isProtected: true
  },
  {
    path: '/vehicleCreate',
    element: <VehicleCreate />,
    isProtected: true
  },
  {
    path: '/offenceCreate',
    element: <OffenceCreate />,
    isProtected: true
  },
  {
    path: '/offencePending',
    element: <OffencePending />,
    isProtected: true
  },
  {
    path: '/theftCreate',
    element: <TheftCreate />,
    isProtected: true
  },
  {
    path: '/theftFound',
    element: <TheftFound />,
    isProtected: true
  }
];

export default AppRoutes;
