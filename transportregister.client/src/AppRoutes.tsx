﻿import { VehicleList } from "./components/vehicle/VehicleList";
import Login from './components/Login';
import DriverSearch from "./components/DriverSearch";
import VehicleCreate from "./components/vehicle/VehicleCreate";
import OffencePending from "./components/offence/OffencePending";
import DriverDetail from "./components/DriverDetail";
import OffenceDetail from "./components/offence/OffenceDetail";
import UserList from "./components/admin/UserList";
import VehicleSearch from "./components/vehicle/VehicleSearch";
import VehicleDetail from "./components/vehicle/VehicleDetail";
import VehicleEdit from "./components/vehicle/VehicleEdit";
import OffenceAll from "./components/offence/OffenceAll";
import { UserDetail } from "./components/user/UserDetail";
import { MyAccount } from "./components/user/MyAccount";
import { TheftDetail } from "./components/theft/TheftDetail";
import { TheftAll } from "./components/theft/TheftAll";
import { TheftActive } from "./components/theft/TheftActive";
import DriverList from "./components/DriverList";
import { MainPage } from "./components/MainPage";

const AppRoutes = [
  {
    path: '/',
    element: <MainPage />,
    isProtected: true,
  },
  {
    path: '/login',
    element: <Login />,
    isProtected: false
  },
  {
    path: '/driverAll',
    element: <DriverList />,
    isProtected: true
  },
  {
    path: '/driverSearch',
    element: <DriverSearch />,
    isProtected: true
  },
  {
    path: '/driver/:id',
    element: <DriverDetail />,
    isProtected: true
  },
  {
    path: '/vehicle/:id',
    element: <VehicleDetail />,
    isProtected: true
  },
  {
    path: '/vehicle/edit/:id',
    element: <VehicleEdit />,
    isProtected: true
  },
  {
    path: '/vehicle/search',
    element: <VehicleSearch />,
    isProtected: true
  },
  {
    path: '/vehicle/create',
    element: <VehicleCreate />,
    isProtected: true
  },
  {
    path: '/vehicles',
    element: <VehicleList />,
    isProtected: true,
  },
  {
    path: '/offencePending',
    element: <OffencePending />,
    isProtected: true
  },
  {
    path: '/offenceAll',
    element: <OffenceAll />,
    isProtected: true
  },
  {
    path: '/offence/:id',
    element: <OffenceDetail />,
    isProtected: true
  },
  {
    path: '/thefts',
    element: <TheftAll />,
    isProtected: true
  },
  {
    path: '/theftsActive',
    element: <TheftActive />,
    isProtected: true
  },
  {
    path: '/theft/:id',
    element: <TheftDetail />,
    isProtected: true
  },
  {
    path: '/users',
    element: <UserList />,
    //isProtected: true
  },
  {
    path: '/user/:id',
    element: <UserDetail />,
    isProtected: true
  },
  {
    path: '/myAccount',
    element: <MyAccount />,
    isProtected: true
  }
];

export default AppRoutes;
