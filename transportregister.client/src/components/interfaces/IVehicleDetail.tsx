import ILicensePlateHistory from "./ILicensePlateHistory";

export interface IVehicleDetail {
  vehicleId: number;
  vin: string;
  currentLicensePlate: string;
  vehicleType: string;
  manufacturer: string;
  model: string;
  color: string;
  horsepowerKW: number;
  engineVolumeCM3: number;
  manufacturedYear: number;
  lengthCM: number;
  widthCM: number;
  heightCM: number;
  loadCapacityKG: number;
  imageBase64?: string;

  currentlyStolenId: number | null;
  currentlyStolen: boolean;

  ownerId: number;
  ownerFullName: string;
  officialId: string;
  officialFullName: string;

  licensePlateHistory: ILicensePlateHistory[];
}

export interface ICar extends IVehicleDetail {
  numberOfDoors: number;
}

export interface ITruck extends IVehicleDetail {
  // Truck specific properties can be added here
}

export interface IMotorcycle extends IVehicleDetail {
  constraints: string;
}

export interface IBus extends IVehicleDetail {
  seatCapacity: number;
  standingCapacity: number;
}
