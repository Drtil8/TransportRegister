import { VehicleList } from "./components/VehicleList";
import { Forecast } from "./components/Forecast";
import Login from './components/Login';

const AppRoutes = [
  {
    path: '/',
    element: <VehicleList />,
    isProtected: true
  },
  {
    path: '/forecast',
    element: <Forecast />,
    isProtected: true
  },
  {
    path: '/login',
    element: <Login />,
    isProtected: false
  }
];

export default AppRoutes;
