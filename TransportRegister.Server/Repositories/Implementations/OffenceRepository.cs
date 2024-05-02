using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.FineDTOs;
using TransportRegister.Server.DTOs.OffenceDTOs;
using TransportRegister.Server.DTOs.PersonDTOs;
using TransportRegister.Server.DTOs.UserDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories.Implementations
{
    public class OffenceRepository : IOffenceRepository
    {
        private readonly AppDbContext _context;

        public OffenceRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<OffenceListItemDto> QueryAllOffences()
        {
            return _context.Offences
                .AsNoTracking()
                .Include(of => of.OffenceType)
                .Include(of => of.CommitedBy)
                .Select(of => new OffenceListItemDto
                {
                    OffenceId = of.OffenceId,
                    ReportedOn = of.ReportedOn,
                    OffenceType = of.OffenceType.Name,
                    IsValid = of.IsValid,
                    IsApproved = of.IsApproved,
                    PenaltyPoints = of.PenaltyPoints,
                    Person = new PersonSimpleDto
                    {
                        PersonId = of.PersonId,
                        FullName = of.CommitedBy.FirstName + " " + of.CommitedBy.LastName,
                        BirthNumber = of.CommitedBy.BirthNumber
                    },
                    Vehicle = of.VehicleId == null ? null :
                    new VehicleSimpleDto
                    {
                        VehicleId = of.OffenceOnVehicle.VehicleId,
                        VIN = of.OffenceOnVehicle.VIN,
                        LicensePlate = of.OffenceOnVehicle.LicensePlates.OrderByDescending(lp => lp.ChangedOn).Select(lp => lp.LicensePlate).FirstOrDefault(),
                        Manufacturer = of.OffenceOnVehicle.Manufacturer,
                        Model = of.OffenceOnVehicle.Model,
                    }
                });
        }

        public IQueryable<OffenceListItemDto> QueryUnresolvedOffences()
        {
            return _context.Offences
                .AsNoTracking()
                .Include(of => of.OffenceType)
                .Include(of => of.CommitedBy)
                .Where(of => !of.IsApproved && of.IsValid)
                .Select(of => new OffenceListItemDto
                {
                    OffenceId = of.OffenceId,
                    ReportedOn = of.ReportedOn,
                    OffenceType = of.OffenceType.Name,
                    IsValid = of.IsValid,
                    IsApproved = of.IsApproved,
                    PenaltyPoints = of.PenaltyPoints,
                    Person = new PersonSimpleDto
                    {
                        PersonId = of.PersonId,
                        FullName = of.CommitedBy.FirstName + " " + of.CommitedBy.LastName,
                        BirthNumber = of.CommitedBy.BirthNumber
                    },
                    Vehicle = of.VehicleId == null ? null : 
                    new VehicleSimpleDto
                    {
                        VehicleId = of.OffenceOnVehicle.VehicleId,
                        VIN = of.OffenceOnVehicle.VIN,
                        LicensePlate = of.OffenceOnVehicle.LicensePlates.OrderByDescending(lp => lp.ChangedOn).Select(lp => lp.LicensePlate).FirstOrDefault(),
                        Manufacturer = of.OffenceOnVehicle.Manufacturer,
                        Model = of.OffenceOnVehicle.Model,
                    }
                });
        }

        public IQueryable<OffenceListItemDto> QueryOffences(bool unresolved)
        {
            var query = _context.Offences
                .AsNoTracking()
                .Include(of => of.OffenceType)
                .Include(of => of.CommitedBy);

            if (unresolved)
            {
                query.Where(of => !of.IsApproved && of.IsValid);
            }

            return query.Select(of => new OffenceListItemDto
            {
                OffenceId = of.OffenceId,
                ReportedOn = of.ReportedOn,
                OffenceType = of.OffenceType.Name,
                IsValid = of.IsValid,
                IsApproved = of.IsApproved,
                PenaltyPoints = of.PenaltyPoints,
                Person = new PersonSimpleDto
                {
                    PersonId = of.PersonId,
                    FullName = of.CommitedBy.FirstName + " " + of.CommitedBy.LastName,
                    BirthNumber = of.CommitedBy.BirthNumber
                },
                Vehicle = of.VehicleId == null ? null :
                    new VehicleSimpleDto
                    {
                        VehicleId = of.OffenceOnVehicle.VehicleId,
                        VIN = of.OffenceOnVehicle.VIN,
                        LicensePlate = of.OffenceOnVehicle.LicensePlates.OrderByDescending(lp => lp.ChangedOn).Select(lp => lp.LicensePlate).FirstOrDefault(),
                        Manufacturer = of.OffenceOnVehicle.Manufacturer,
                        Model = of.OffenceOnVehicle.Model,
                    }
            });
        }

        public IQueryable<OffenceListItemDto> ApplyFilterQueryOffences(IQueryable<OffenceListItemDto> query, DtParamsDto dtParams)
        {
            //foreach(var filter in dtParams.Filters)
            //{

            //}

            return query;
        }

        public async Task<OffenceDetailDto> GetOffenceByIdAsync(int offenceId, User user)
        {
            var offenceDto = await _context.Offences
                .Where(of => of.OffenceId == offenceId)
                .Include(of => of.OffenceOnVehicle)
                .Include(of => of.OffenceOnVehicle.LicensePlates)
                .Include(of => of.Fine)
                .Include(of => of.OffenceType)
                .Select(of => new OffenceDetailDto
                {
                    OffenceId = of.OffenceId,
                    ReportedOn = of.ReportedOn,
                    //Address = of.Address, // TODO
                    Type = of.OffenceType.Name,
                    IsValid = of.IsValid,
                    IsApproved = of.IsApproved,
                    Description = of.Description,
                    Vehicle = of.VehicleId == null ?
                    null :
                    new VehicleSimpleDto // TODO -> doesnt have to be specified, can be null
                    {
                        VehicleId = of.OffenceOnVehicle.VehicleId,
                        Manufacturer = of.OffenceOnVehicle.Manufacturer,
                        Model = of.OffenceOnVehicle.Model,
                        VIN = of.OffenceOnVehicle.VIN,
                        LicensePlate = of.OffenceOnVehicle.LicensePlates.OrderByDescending(lp => lp.ChangedOn).Select(lp => lp.LicensePlate).FirstOrDefault()
                    },
                    PenaltyPoints = of.PenaltyPoints,
                    IsResponsibleOfficial = of.OfficialId == user.Id,
                    Person = new PersonSimpleDto
                    {
                        PersonId = of.PersonId,
                        FullName = of.CommitedBy.FirstName + " " + of.CommitedBy.LastName,
                        BirthNumber = of.CommitedBy.BirthNumber
                    },
                    Officer = new UserSimpleDto
                    {
                        Id = of.OfficerId,
                        FullName = of.ReportedByOfficer.FirstName + " " + of.ReportedByOfficer.LastName
                    },
                    Official = of.OfficialId == null ? null : new UserSimpleDto
                    {
                        Id = of.OfficialId,
                        FullName = of.ProcessedByOfficial.FirstName + " " + of.ProcessedByOfficial.LastName
                    }
                    // TODO -> person dto
                }).FirstOrDefaultAsync();

            var fine = await _context.Fines
                .Where(f => f.OffenceId == offenceId)
                .Select(f => new FineDetailDto
                {
                    FineId = f.FineId,
                    Amount = f.Amount,
                    PaidOn = f.PaidOn,
                    DueDate = f.DueDate,
                    IsActive = f.IsActive,
                    IsPaid = !f.IsActive
                }).FirstOrDefaultAsync();

            if (fine != null)
            {
                offenceDto.Fine = fine;
            }

            var photos = await _context.OffencePhotos
                .Where(op => op.OffenceId == offenceId)
                .Select(op => op.Image)
                .ToListAsync();

            if (photos != null)
            {
                foreach (var photo in photos)
                {
                    offenceDto.OffencePhotos64.Add(Convert.ToBase64String(photo));
                }
            }

            if (offenceDto == null)
            {
                return null;
            }

            return offenceDto;
        }

        public async Task<IEnumerable<OffenceTypeDto>> GetOffenceTypesAsync()
        {
            var offenceTypes = await _context.OffenceTypes
                .Select(ot => new OffenceTypeDto
                {
                    Id = ot.OffenceTypeId,
                    Name = ot.Name,
                    PenaltyPoints = ot.PenaltyPoints,
                    FineAmount = ot.FineAmount
                }).ToListAsync();

            return offenceTypes;
        }

        public async Task<bool> AssignPoints(int driverId, int points)
        {
            var driver = await _context.Drivers.FindAsync(driverId);
            if (driver == null)
            {
                return false;
            }

            driver.BadPoints += points;
            driver.LastCrimeCommited = DateTime.Now; // TODO -> rn sets last crime commited only when points are assigned!! check if its okay
            if (driver.BadPoints >= 12)
            {
                driver.HasSuspendedLicense = true;
                driver.DrivingSuspendedUntil = DateTime.Now.AddYears(1);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Offence> ReportOffenceAsync(OffenceCreateDto offenceDto, User activeUser)
        {
            // TODO -> implement
            var offence = new Offence
            {
                OffenceTypeId = offenceDto.OffenceTypeId,
                ReportedOn = DateTime.Now,
                Description = offenceDto.Description,
                PenaltyPoints = offenceDto.PenaltyPoints,
                //Address = new Address // TODO
                //{
                //    City = offenceDto.Address.City,
                //    Street = offenceDto.Address.Street,
                //    State = offenceDto.Address.State,
                //    Country = offenceDto.Address.Country,
                //    HouseNumber = offenceDto.Address.HouseNumber,
                //    PostalCode = offenceDto.Address.PostalCode
                //},
                IsApproved = false,
                IsValid = true,
                VehicleId = offenceDto.VehicleId != 0 ? offenceDto.VehicleId : null,
                PersonId = offenceDto.PersonId,
                OfficerId = activeUser.Id
            };

            if (offenceDto.FineAmount != 0)
            {
                offence.Fine = new Fine
                {
                    Amount = offenceDto.FineAmount,
                    IsActive = true,
                    DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7))
                };

                if (offenceDto.FinePaid)
                {
                    offence.Fine.PaidOn = DateOnly.FromDateTime(offence.ReportedOn);
                    offence.Fine.IsActive = false;
                    offence.IsApproved = true; // Automatically processing offence when it was dealt with on place
                    if (offence.PenaltyPoints > 0)
                    {
                        await AssignPoints(offence.PersonId, offence.PenaltyPoints);
                    }
                }
            }
            else // TODO -> no penalty should mean not points but maybe i should check the points as well
            {
                offence.IsApproved = true; // Automatically processing offence when no fine is assigned
            }


            _context.Offences.Add(offence);
            var res = await _context.SaveChangesAsync();
            if (res == 0)
            {
                return null;
            }

            if(offenceDto.Photos != null)
            {
                foreach (var photo in offenceDto.Photos)
                {
                    if(!string.IsNullOrEmpty(photo))
                    {
                        var image = Convert.FromBase64String(photo);
                        var offencePhoto = new OffencePhoto
                        {
                            Image = image,
                            OffenceId = offence.OffenceId
                        };
                        _context.OffencePhotos.Add(offencePhoto);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return offence;
        }

        public async Task<bool> ApproveOffenceAsync(int offenceId, string officialId, OffenceDetailDto offenceDto)
        {
            var offence = await _context.Offences.Include(of => of.Fine).Where(of => of.OffenceId == offenceId).FirstOrDefaultAsync();
            if (offence == null)
            {
                return false;
            }

            if(offence.Fine != null)
            {
                if(offence.Fine.IsActive) // TODO 
                {
                    offence.Fine.IsActive = false;
                    offence.Fine.PaidOn = DateOnly.FromDateTime(DateTime.Now);
                }
            }

            offence.IsApproved = true;
            offence.IsValid = true;
            offence.OfficialId = officialId;

            if(await _context.SaveChangesAsync() > 0)
            {
                offenceDto.IsApproved = offence.IsApproved;
                offenceDto.IsValid = offence.IsValid;
                offenceDto.Official = new UserSimpleDto
                {
                    Id = officialId,
                    FullName = offence.ProcessedByOfficial.FirstName + " " + offence.ProcessedByOfficial.LastName
                };
                if(offenceDto.Fine != null)
                {
                    offenceDto.Fine.IsPaid = !offence.Fine.IsActive;
                    offenceDto.Fine.IsActive = offence.Fine.IsActive;
                    offenceDto.Fine.PaidOn = offence.Fine.PaidOn;
                }
            }
            else
            {
                return false;
            }
            
            if(offence.PenaltyPoints > 0)
            {
                var res = await AssignPoints(offence.PersonId, offence.PenaltyPoints);
            }

            return true;
        }

        public async Task<bool> DeclineOffenceAsync(int offenceId, string officialId)
        {
            var offence = await _context.Offences.FindAsync(offenceId);
            if (offence == null)
            {
                return false;
            }

            offence.IsApproved = false;
            offence.IsValid = false;
            offence.OfficialId = officialId;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> EditOffenceAsync(int offenceId, OffenceDetailDto offenceDto) // TODO
        {
            var offence = await _context.Offences.FindAsync(offenceId);
            if (offence == null)
            {
                return -1;
            }

            offence.Description = offenceDto.Description;
            //offence.PenaltyPoints = offenceDto.PenaltyPoints; // TODO
            offence.Fine.Amount = offenceDto.Fine.Amount;
            //offence.Fine.IsActive = !offenceDto.Fine.IsPaid; // TODO
            return -1;
        }

        public async Task<bool> DeleteOffenceAsync(int offenceId) // TODO
        {
            var offence = await _context.Offences.FindAsync(offenceId);
            if (offence == null)
            {
                return false;
            }

            _context.Offences.Remove(offence);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
