import IFineDetail from "./IFineDetail";
import IVehicleListItem from "./IVehicleListItem";

interface IOffenceDetail {
  offenceId: number;
  reportedOn: Date;
  //location: string; // todo
  type: string;
  isValid: boolean;
  isApproved: boolean;
  description: string;
  vehicle: IVehicleListItem | null; // todo
  penaltyPoints: number;
  fine: IFineDetail | null;
  //photos: string[]; // todo
  isResponsibleOfficial: boolean;
}

export default IOffenceDetail;
