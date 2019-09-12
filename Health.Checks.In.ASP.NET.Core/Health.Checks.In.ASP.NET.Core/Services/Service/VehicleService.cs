using Health.Checks.In.ASP.NET.Core.Model;
using Health.Checks.In.ASP.NET.Core.Repositories.Interface;
using Health.Checks.In.ASP.NET.Core.Services.Interfaces;
using System.Threading.Tasks;

namespace Health.Checks.In.ASP.NET.Core.Services.Service
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task AddVehicle(Vehicle item)
        {
            await _vehicleRepository.AddVehicle(item);
        }

        public async Task<Vehicle> GetVehicle(string id)
        {
            return await _vehicleRepository.GetVehicle(id);
        }
    }
}
