﻿using Microsoft.AspNetCore.Mvc;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.DriversLicenseDTOs;
using TransportRegister.Server.DTOs.PersonDTOs;
using TransportRegister.Server.Models;
namespace TransportRegister.Server.Repositories
{
    public interface IPersonRepository
    {
        Task<Person> GetPersonByIdAsync(int personId);
        Task<Person> GetPersonByBirthNumberAsync(string birthNumber);
        Task<Driver> GetDriverAsync(string licenseNumber);
        Task<Person> GetOwnerByVINAsync(string VIN_number);
        Task AddDriverAsync(int personId, string license);
        Task DeletePersonAsync(int personId);
        Task SavePersonAsync(Person person);
        Task SaveDriverAsync(Driver driver);
        Task<List<Tuple<Driver, int>>> GetDriversAndPoints();
        Task<List<Theft>> GetPersonReportedTheftsByIdAsync(int personId);
        Task<List<Offence>> GetPersonCommitedOffencesByIdAsync(int personId);
        Task AddDriversLicense(int driverId, string official_id, DriversLicenseCreateDto license);

        public IQueryable<DriverSimpleListDto> QueryAllPersons(DtParamsDto dtParams);
    }
}
