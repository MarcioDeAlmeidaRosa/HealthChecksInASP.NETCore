using Health.Checks.In.ASP.NET.Core.Repositories.Interface;
using Health.Checks.In.ASP.NET.Core.Repositories.Repository;
using Health.Checks.In.ASP.NET.Core.Services.Interfaces;
using Health.Checks.In.ASP.NET.Core.Services.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Checks.In.ASP.NET.Core.IoC
{
    public static class ConfigureIoC
    {
        public static IServiceCollection RegisterIoC(this IServiceCollection services)
        {
            services.AddTransient<IVehicleService, VehicleService>();
            services.AddTransient<IVehicleRepository, VehicleRepository>();

            return services;
        }
    }
}
