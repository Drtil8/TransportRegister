import IPersonDetail from "./IPersonDetail";
import IVehicleListItem from "./IVehicleListItem";

interface IDriverDetail {
  person: IPersonDetail;
  driversLicenseNumber: string;
  badPoints: number;
  hasSuspendedLicense: boolean;
  lastCrimeCommited: string;
  drivingSuspendedUntil: string;
  licenses: any[]; // Adjust this type based on your `DriversLicense` interface
  vehicles: IVehicleListItem[];
}

export default IDriverDetail;
