using Microsoft.OpenApi.Extensions;
using TransportRegister.Server.DTOs.PersonDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.DTOs.DriversLicenseDTOs
{
    public class DriversLicenseDtoTransformer
    {
        public static DriversLicenseDto TransformToDto(DriversLicense license)

        {
            if (license == null)
                return null;

            DriversLicenseDto licenseDto = new DriversLicenseDto
            {
                DriversLicenseId = license.DriversLicenseId,
                IssuedOn = license.IssuedOn,
                VehicleType = license.VehicleType.GetDisplayName(),
                DriverId = license.DriverId,
            };

            return licenseDto;

        }

        public static DriversLicense TransformToEntity(DriversLicenseDto licenseDto)

        {
    
            DriversLicense license = new DriversLicense
            {
                DriversLicenseId = licenseDto.DriversLicenseId,
                IssuedOn = licenseDto.IssuedOn,
                VehicleType = Enum.Parse<VehicleType>(licenseDto.VehicleType),
                DriverId = licenseDto.DriverId
            };

            return license;

        }
    }
}
