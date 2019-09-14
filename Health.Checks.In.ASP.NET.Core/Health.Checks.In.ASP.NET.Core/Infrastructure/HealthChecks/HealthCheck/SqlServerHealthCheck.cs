using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Health.Checks.In.ASP.NET.Core.Infrastructure.HealthChecks.HealthCheck
{
    public class SqlServerHealthCheck : IHealthCheck
    {
        SqlConnection _connection;

        public string Name => "sql";

        public SqlServerHealthCheck(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                _connection.Open();
            }
            catch (SqlException)
            {
                return HealthCheckResult.Unhealthy();
            }

            return HealthCheckResult.Healthy();
        }
    }
}
