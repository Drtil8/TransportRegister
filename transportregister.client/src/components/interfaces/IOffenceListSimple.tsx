interface IOffenceListSimple {
  offenceId: number;
  reportedOn: Date;
  description: string;
  penaltyPoints: number;
  fineAmount: number;
}

export default IOffenceListSimple;
