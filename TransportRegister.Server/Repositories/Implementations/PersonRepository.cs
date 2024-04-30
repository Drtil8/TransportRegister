﻿using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories.Implementations
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _context;

        public PersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Person> GetPersonByIdAsync(int personId)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(v => v.PersonId == personId);
            if (person is null)
            {
                return null;
            }

            var type = person.GetType();

            if (person.GetType() == typeof(Driver))
            {
                return await _context.Drivers
                    .Include(v => v.Licenses)
                    .FirstOrDefaultAsync(v => v.PersonId == personId);
            }
            else if (person.GetType() == typeof(Owner))
            {
                return await _context.Owners
                    .Include(v => v.Vehicles)
                    .FirstOrDefaultAsync(v => v.PersonId == personId);
            }
            else return null;
        }
        public async Task<Driver> GetDriverAsync(string licenseNumber)
        {
            return await _context.Drivers

                .FirstOrDefaultAsync(v => v.DriversLicenseNumber == licenseNumber);
        }
        public async Task<Owner> GetOwnerByVINAsync(string VIN_number)
        {
            return await _context.Owners
                .Include(o => o.Vehicles)
                .FirstOrDefaultAsync(o => o.Vehicles.Any(v => v.VIN == VIN_number));
        }

        public async Task SetOwnerAsync(Person owner)
        {
            if (owner.PersonId == 0)
            {
                _context.Owners.Add(owner as Owner);
            }
            else
            {
                _context.Owners.Update(owner as Owner);
            }
            await _context.SaveChangesAsync();
        }


        public async Task SetDriverAsync(Person driver)
        {
            if (driver.PersonId == 0)
            {
                _context.Drivers.Add(driver as Driver);
            }
            else
            {
                _context.Drivers.Update(driver as Driver);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeletePersonAsync(int personId)
        {
            Person person = await _context.Persons.FirstOrDefaultAsync(d => d.PersonId == personId);
            if (person != null)
            {
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Tuple<Driver, int>>> GetDriversAndPoints()
        {
            var driversWithPoints = await _context.Drivers
                .Select(driver => new { Driver = driver, Points = driver.BadPoints }) // Select drivers and their points
                .ToListAsync();

            var result = driversWithPoints
                .Select(item => Tuple.Create(item.Driver, item.Points))
                .ToList();

            return result;
        }
    }
}