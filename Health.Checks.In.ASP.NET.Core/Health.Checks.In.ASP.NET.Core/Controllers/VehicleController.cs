using Health.Checks.In.ASP.NET.Core.Model;
using Health.Checks.In.ASP.NET.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Health.Checks.In.ASP.NET.Core.Controllers
{
    [Route("api/Vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("{id}")]
        public async Task<Vehicle> Get(string id)
        {
            return await _vehicleService.GetVehicle(id);
        }

        [HttpPost]
        public async Task Post([FromBody] Vehicle vehicle)
        {
            await _vehicleService.AddVehicle(vehicle);
        }
    }
}