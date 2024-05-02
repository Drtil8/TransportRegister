import IFineDetail from "./IFineDetail";
import IPersonSimple from "./IPersonSimple";
import IUserSimple from "./IUserSimple";
import IVehicleSimple from "./IVehicleSimple";

interface IOffenceDetail {
  offenceId: number;
  reportedOn: Date;
  address: string;
  type: string;
  isValid: boolean;
  isApproved: boolean;
  description: string;
  vehicle: IVehicleSimple | null;
  person: IPersonSimple;
  officer: IUserSimple;
  official: IUserSimple | null;
  penaltyPoints: number;
  fine: IFineDetail | null;
  isResponsibleOfficial: boolean;
  offencePhotos64: string[];
}

export default IOffenceDetail;
