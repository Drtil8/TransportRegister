interface IVehicleListItem {
  id: number;
  vin: string;
  licensePlate: string;
  vehicleType: string;
  manufacturer: string;
  model: string;
  color: string;
  manufacturedYear: number;
  ownerId: number;
  ownerFullName: string;
}

export default IVehicleListItem;
