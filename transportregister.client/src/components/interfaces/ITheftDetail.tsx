import IUserSimple from "./IUserSimple";
import IVehicleListItem from "./IVehicleListItem";

interface ITheftDetail {
  theftId: number;
  address: string;
  stolenOn: Date;
  reportedOn: Date;
  foundOn?: Date | null;
  returnedOn?: Date | null;
  description: string;
  isFound: boolean;
  isReturned: boolean;
  vehicleId: number;
  stolenVehicle: IVehicleListItem;
  official: IUserSimple | null;
  officerReported: IUserSimple | null;
  officerFound: IUserSimple | null;
}

export default ITheftDetail;
