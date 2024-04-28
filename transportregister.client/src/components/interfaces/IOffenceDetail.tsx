import IFineDetail from "./IFineDetail";
import IVehicleDetail from "./IVehicleDetail";

interface IOffenceDetail {
  offenceId: number;
  reportedOn: Date;
  //location: string; // todo
  type: string;
  isValid: boolean;
  isApproved: boolean;
  description: string;
  vehicle: IVehicleDetail | null; // todo
  penaltyPoints: number;
  fine: IFineDetail | null;
  //photos: string[]; // todo
}

export default IOffenceDetail;
