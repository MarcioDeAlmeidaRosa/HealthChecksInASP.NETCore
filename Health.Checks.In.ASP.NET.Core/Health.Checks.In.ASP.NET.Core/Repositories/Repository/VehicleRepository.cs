using Health.Checks.In.ASP.NET.Core.Infrastructure.Data;
using Health.Checks.In.ASP.NET.Core.Infrastructure.Data.Model;
using Health.Checks.In.ASP.NET.Core.Model;
using Health.Checks.In.ASP.NET.Core.Repositories.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Health.Checks.In.ASP.NET.Core.Repositories.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly DBContext _context = null;

        public VehicleRepository(IOptions<Settings> settings)
        {
            _context = new DBContext(settings);
        }

        private ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }

        public async Task AddVehicle(Vehicle item)
        {
            try
            {
                await _context.Vehicles.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<Vehicle> GetVehicle(string id)
        {
            try
            {
                ObjectId _id = GetInternalId(id);
                return await _context.Vehicles.Find(note => note.ID == _id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
