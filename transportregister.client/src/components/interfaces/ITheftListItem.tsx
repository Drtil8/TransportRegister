interface ITheftListItem {
  theftId: number;
  vehicleId: number;
  vin: string;
  licensePlate: string;
  stolenOn: Date;
  reportedOn: Date;
  foundOn: Date | null;
  isFound: boolean;
}

export default ITheftListItem;
