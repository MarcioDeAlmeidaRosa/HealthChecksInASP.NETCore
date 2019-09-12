using Health.Checks.In.ASP.NET.Core.Model;
using System.Threading.Tasks;

namespace Health.Checks.In.ASP.NET.Core.Services.Interfaces
{
    public interface IVehicleService
    {
        Task AddVehicle(Vehicle item);

        Task<Vehicle> GetVehicle(string id);
    }
}
