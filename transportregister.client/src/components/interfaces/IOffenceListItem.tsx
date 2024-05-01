import IPersonSimple from "./IPersonSimple";
import IVehicleSimple from "./IVehicleSimple";

interface IOffenceListItem {
  offenceId: number;
  reportedOn: Date;
  offenceType: string;
  isValid: boolean;
  isApproved: boolean;
  penaltyPoints: number;
  vehicle: IVehicleSimple | null;
  person: IPersonSimple;
}

export default IOffenceListItem;
