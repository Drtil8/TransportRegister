interface IVehicleDetail {
  vehicleId: number;
  VIN: string;
  licensePlate: string;
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

  // todo probably load whole interface
  ownerId: number;
  officialFullName: string;
  officialId: string;
  ownerFullName: string;
}

export default IVehicleDetail;
