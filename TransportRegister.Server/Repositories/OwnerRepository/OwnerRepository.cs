using TransportRegister.Server.Data;
using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Models;
namespace TransportRegister.Server.Repositories.OwnerRepository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly AppDbContext _context;

        public OwnerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Owner> GetOwnerByVINAsync(string VIN_number)
        {
            return await _context.Owners
                .Include(o => o.Vehicles)
                .FirstOrDefaultAsync(o => o.Vehicles.Any(v => v.VIN == VIN_number));
        }

        public async Task SaveOwnerAsync(Owner owner)
        {
            if (owner.PersonId == 0)
            {
                _context.Owners.Add(owner);
            }
            else
            {
                _context.Owners.Update(owner);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOwnerAsync(int ownerId)
        {
            Owner owner = await _context.Owners.FirstOrDefaultAsync(o => o.PersonId == ownerId);
            if (owner != null)
            {
                _context.Owners.Remove(owner);
                await _context.SaveChangesAsync();
            }
        }

        public async Task <Owner> GetOwnerByIdAsync(int ownerId)
        {
            return await _context.Owners
                .Include(o => o.Vehicles)
                .FirstOrDefaultAsync(o => o.PersonId == ownerId);
        }


    }
}
