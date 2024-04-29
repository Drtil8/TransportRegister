

interface IDriverFormState {
  firstName: string;
  lastName: string;
  birthNumber1: number; // before slash
  birthNumber2: number; // after slash
  sexMale: boolean;
  dateOfBirth: string;
  //address: {
  //  street: string;
  //  city: string;
  //  state: string;
  //  country: string;
  //  houseNumber: string;
  //  postalCode: string;
  //};
  image: string; //TODO CHANGE TYPE
  //officialId: string;
  driversLicenseNumber: string;
  badPoints: number;
  hasSuspendedLicense: boolean;
  lastCrimeCommited: string;
  drivingSuspendedUntil: string;
  licenses: any[]; // Adjust this type based on your `DriversLicense` interface

  disableInput: boolean;
}

export default IDriverFormState;
