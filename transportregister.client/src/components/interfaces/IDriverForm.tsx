interface IDriverFormState {
  driversLicenseNumber: string;
  badPoints: number;
  hasSuspendedLicense: boolean;
  lastCrimeCommited: string;
  drivingSuspendedUntil: string;

  disableInput: boolean;
  licensesStrings: string[];
}

export default IDriverFormState;
