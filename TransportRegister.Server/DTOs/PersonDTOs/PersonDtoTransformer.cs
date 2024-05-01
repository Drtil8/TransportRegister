﻿using Humanizer;
using System.Linq;
using TransportRegister.Server.DTOs.DriversLicenseDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.DTOs.OffenceDTOs;
using TransportRegister.Server.DTOs.TheftDTOs;
using TransportRegister.Server.Models;
namespace TransportRegister.Server.DTOs.PersonDTOs
{
    public class PersonDtoTransformer
    {
        public static PersonDto TransformToDto(Person person)
        {
            if (person == null)
                return null;

            PersonDto personDto = person switch
            {
                Driver driver => new DriverDto
                {
                    DriversLicenseNumber = driver.DriversLicenseNumber,
                    BadPoints = driver.BadPoints,
                    HasSuspendedLicense = driver.HasSuspendedLicense,
                    LastCrimeCommited = driver.LastCrimeCommited,
                    DrivingSuspendedUntil = driver.DrivingSuspendedUntil,
                    Licenses = driver.Licenses.Select(l => DriversLicenseDtoTransformer.TransformToDto(l)),
                },
                Owner owner => new OwnerDto
                {
                    Vehicles = owner.Vehicles.Select(v =>
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
                        }
                    ),
                },
                _ => null
            };

            if (personDto != null)
            {
                personDto.AddressDto = TransformToDto(person.Address);
                personDto.PersonId = person.PersonId;
                personDto.FirstName = person.FirstName;
                personDto.LastName = person.LastName;
                personDto.BirthNumber = person.BirthNumber;
                personDto.Sex_Male = person.Sex_Male;
                personDto.ImageBase64 = person.Image != null ? Convert.ToBase64String(person.Image) : null;
                personDto.OfficialId = person.OfficialId;
                personDto.PersonType = person.GetType().Name;

                personDto.CommitedOffences = person.CommitedOffences.Select(o => new OffenceListSimpleDto
                {
                    OffenceId = o.OffenceId,
                    ReportedOn = o.ReportedOn,
                    Description = o.Description,
                    PenaltyPoints = o.PenaltyPoints,
                    FineAmount = o.Fine is not null ? o.Fine.Amount : default,
                });
                personDto.ReportedThefts = person.ReportedThefts.Select(t => new TheftListItemDto
                {
                    TheftId = t.TheftId,
                    ReportedOn = t.ReportedOn,
                    VehicleId = t.VehicleId,
                    VIN = t.StolenVehicle.VIN,
                    LicensePlate = t.StolenVehicle.LicensePlates.FirstOrDefault().ToString(),
                    StolenOn = t.StolenOn,
                    FoundOn = t.FoundOn,
                    IsFound = t.FoundOn != null,
                });
            };
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
                OwnerDto ownerDto => new Owner
                {
                    Address = TransformToEntity(ownerDto.AddressDto),
                    PersonId = ownerDto.PersonId,
                    FirstName = ownerDto.FirstName,
                    LastName = ownerDto.LastName,
                    BirthNumber = ownerDto.BirthNumber,
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
