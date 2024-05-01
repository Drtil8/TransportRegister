using Humanizer;
using System.Linq;
using TransportRegister.Server.DTOs.DriversLicenseDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.DTOs.OffenceDTOs;
using TransportRegister.Server.DTOs.TheftDTOs;
using TransportRegister.Server.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace TransportRegister.Server.DTOs.PersonDTOs
{
    public class PersonDtoTransformer
    {
        public static PersonDto TransformToDto(Person person)
        {
            if (person == null)
                return null;

            PersonDto personDto = new PersonDto { };

            if (person is Driver driver)
            {
                personDto = new DriverDto
                {
                    DriversLicenseNumber = driver.DriversLicenseNumber,
                    BadPoints = driver.BadPoints,
                    HasSuspendedLicense = driver.HasSuspendedLicense,
                    LastCrimeCommited = driver.LastCrimeCommited,
                    DrivingSuspendedUntil = driver.DrivingSuspendedUntil,
                    Licenses = driver.Licenses.Select(l => DriversLicenseDtoTransformer.TransformToDto(l)),
                };
            }


            personDto.AddressDto = TransformToDto(person.Address);
            personDto.PersonId = person.PersonId;
            personDto.FirstName = person.FirstName;
            personDto.LastName = person.LastName;
            personDto.BirthNumber = person.BirthNumber;
            personDto.Sex_Male = person.Sex_Male;
            personDto.ImageBase64 = person.Image != null ? Convert.ToBase64String(person.Image) : null;
            personDto.OfficialId = person.OfficialId;
            personDto.PersonType = person.GetType().Name;
            personDto.Vehicles = person.Vehicles
                .Select(v =>
                new VehicleListItemDto
                {
                    Id = v.VehicleId,
                    VIN = v.VIN,
                    VehicleType = v.GetType().Name,
                    LicensePlate = v.LicensePlates
                                    .OrderByDescending(lp => lp.ChangedOn)
                                    .Select(lp => lp.LicensePlate)
                                    .FirstOrDefault(),
                    Manufacturer = v.Manufacturer,
                    Model = v.Model,
                    Color = v.Color,
                    ManufacturedYear = v.ManufacturedYear,
                });
            

            return personDto;
        }


        public static AddressDto TransformToDto(Address adress)
        {
            return new AddressDto
            {
                Street = adress.Street,
                City = adress.City,
                State = adress.State,
                Country = adress.Country,
                HouseNumber = adress.HouseNumber,
                PostalCode = adress.PostalCode
            };
        }

        public static Person TransformToEntity(PersonDto dto)
        {
            return dto switch
            {
                DriverDto driverDto => new Driver
                {
                    Address = TransformToEntity(driverDto.AddressDto),
                    PersonId = driverDto.PersonId,
                    FirstName = driverDto.FirstName,
                    LastName = driverDto.LastName,
                    BirthNumber = driverDto.BirthNumber,
                },
                _ => null
            };
        }

        public static Address TransformToEntity(AddressDto dto)
        {
            return new Address
            {
                Street = dto.Street,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                HouseNumber = dto.HouseNumber,
                PostalCode = dto.PostalCode
            };
        }
    }
}
