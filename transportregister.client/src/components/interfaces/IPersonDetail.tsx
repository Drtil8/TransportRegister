import IAddress from "./IAddress";
import IDriversLicense from "./IDriversLicense";
import { IVehicleDetail } from "./IVehicleDetail";

export interface IPerson {
  personId: number;
  firstName: string;
  lastName: string;
  birthNumber: string;
  sexMale: boolean;
  dateOfBirth: Date;
  address: IAddress;
  imageBase64: string;
  officialId: string;
  personType: string;
}

export interface IDriver extends IPerson {
  driversLicenseNumber: string;
  badPoints: number;
  hasSuspendedLicense: boolean;
  lastCrimeCommited: Date | null;
  drivingSuspendedUntil: Date | null;
  licenses: IDriversLicense[];
}

export interface IOwner extends IPerson {
  vehicles: IVehicleDetail[];
}
