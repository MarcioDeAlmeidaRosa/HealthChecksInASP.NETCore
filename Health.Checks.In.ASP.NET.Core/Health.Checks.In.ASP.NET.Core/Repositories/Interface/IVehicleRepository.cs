using Health.Checks.In.ASP.NET.Core.Model;
using System.Threading.Tasks;

namespace Health.Checks.In.ASP.NET.Core.Repositories.Interface
{
    public interface IVehicleRepository
    {
        Task AddVehicle(Vehicle item);

        Task<Vehicle> GetVehicle(string id);
    }
}
