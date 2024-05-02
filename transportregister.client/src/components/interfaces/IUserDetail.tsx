interface IUserDetail {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
  isValid: boolean;
  personalId: number | null;
  rank: string;
}

export default IUserDetail;
