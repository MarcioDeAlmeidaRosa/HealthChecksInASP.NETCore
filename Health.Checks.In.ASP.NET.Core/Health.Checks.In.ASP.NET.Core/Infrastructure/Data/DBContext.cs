using Health.Checks.In.ASP.NET.Core.Infrastructure.Data.Model;
using Health.Checks.In.ASP.NET.Core.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Health.Checks.In.ASP.NET.Core.Infrastructure.Data
{
    public class DBContext
    {
        private readonly IMongoDatabase _database = null;

        public DBContext(IOptions<Settings> settings)
        {
            _database = new MongoClient(settings.Value.ConnectionString)
                ?.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Vehicle> Vehicles
        {
            get
            {
                return _database.GetCollection<Vehicle>("Vehicles");
            }
        }
    }
}
