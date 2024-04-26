import IVehicleListItem from "./IVehicleListItem";

interface ITheftDetail {
  theftId: number;
  stolenOn: Date;
  reportedOn: Date;
  foundOn?: Date | null;
  description: string;
  isFound: boolean;
  vehicleId: number;
  stolenVehicle: IVehicleListItem;
}

export default ITheftDetail;
