import IFineDetail from "./IFineDetail";
import IPersonSimple from "./IPersonSimple";
import IUserSimple from "./IUserSimple";
import IVehicleSimple from "./IVehicleSimple";

interface IOffenceDetail {
  offenceId: number;
  reportedOn: Date;
  //location: string; // todo
  type: string;
  isValid: boolean;
  isApproved: boolean;
  description: string;
  vehicle: IVehicleSimple | null; // todo
  person: IPersonSimple;
  officer: IUserSimple;
  official: IUserSimple | null;
  penaltyPoints: number;
  fine: IFineDetail | null;
  //photos: string[]; // todo
  isResponsibleOfficial: boolean;
}

export default IOffenceDetail;
