interface IFineDetail {
  fineId: number,
  amount: number,
  isActive: boolean,
  isPaid: boolean,
  paidOn: Date,
  dueDate: Date,
}

export default IFineDetail;
