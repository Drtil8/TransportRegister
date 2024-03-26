import { VehicleList } from "./components/VehicleList";
import { Forecast } from "./components/Forecast";

const AppRoutes = [
  {
    index: true,
    element: <VehicleList />
  },
  {
    path: '/forecast',
    element: <Forecast />
  }
];

export default AppRoutes;
