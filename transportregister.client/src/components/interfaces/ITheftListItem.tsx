import IVehicleSimple from "./IVehicleSimple";

interface ITheftListItem {
  theftId: number;
  vehicle: IVehicleSimple | null;
  //vehicleId: number;
  //vin: string;
  //licensePlate: string;
  stolenOn: Date;
  reportedOn: Date;
  foundOn: Date | null;
  isFound: boolean;
  returnedOn: Date | null;
  isReturned: boolean;
}

export default ITheftListItem;
