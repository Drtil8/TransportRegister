import IPersonDetail from "./IPersonDetail";

interface IDriverDetail {
  person: IPersonDetail;
  driversLicenseNumber: string;
  badPoints: number;
  hasSuspendedLicense: boolean;
  lastCrimeCommited: string;
  drivingSuspendedUntil: string;
  licenses: any[]; // Adjust this type based on your `DriversLicense` interface
}

export default IDriverDetail;
