using Elmah.Io.AspNetCore.HealthChecks;
using Elmah.Io.Client;
using Health.Checks.In.ASP.NET.Core.Infrastructure.HealthChecks.HealthCheck;
using Health.Checks.In.ASP.NET.Core.Infrastructure.HealthChecks.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Mime;

namespace Health.Checks.In.ASP.NET.Core.Infrastructure.HealthChecks
{
    public static class ConfigureHealthCheck
    {
        public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            //Adicionando Middleware para checar a saúde do serviço
            services.AddSingleton<StartupHostedServiceHealthCheck>();
            services.AddHostedService<StartupHostedService>();

            services.AddHealthChecks()
                .AddMemoryHealthCheck("memory_check",
                                      thresholdInBytes: Convert.ToInt64(configuration.GetSection("MemoryCheckOptions:Threshold").Value));
            services.AddHealthChecks()
                .AddCheck<StartupHostedServiceHealthCheck>("slow_dependency_check",
                                                           failureStatus: HealthStatus.Degraded);
            services.AddHealthChecks()
                .AddMongoDb(configuration["MongoConnection:ConnectionString"].ToString(),
                            configuration["MongoConnection:Database"].ToString(),
                            name: "mongoDB_check",
                            failureStatus: HealthStatus.Degraded);

            //Utilização do SQL server
            //services.AddHealthChecks()
            //    .AddCheck<SqlServerHealthCheck>("sql");
            //services.AddSingleton<SqlServerHealthCheck>();

            return services;
        }

        public static IServiceCollection ConfigureElmahChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ElmahIoOptions>(configuration.GetSection("ElmahIo"));

            //Adicionando Middleware para checar a saúde do serviço
            services.AddHealthChecks()
                .AddElmahIoPublisher(configuration.GetSection("ElmahIo:ApiKey").Value,
                                     new Guid(configuration.GetSection("ElmahIo:LogId").Value),
                                     "SUPORTE TI");

            //services.AddElmahIo(o =>
            //{
            //    o.ApiKey = "API_KEY";
            //    o.LogId = new Guid("LOG_ID");
            //    o.OnMessage = message =>
            //    {
            //        message.Version = "42";
            //    };
            //    o.OnError = (message, exception) =>
            //    {
            //        logger.LogError(1, exception, "Error during log to elmah.io");
            //    };
            //});
            //services.AddElmahIo();


            //services.AddElmahIo(o =>
            //{
            //    o.ApiKey = configuration.GetSection("ElmahIo:ApiKey").Value;
            //    o.LogId = new Guid(configuration.GetSection("ElmahIo:LogId").Value);
            //});

            //Configura o envio para o ElmahIo
            services.AddHealthChecks()
                .AddApplicationInsightsPublisher();

            services.AddHealthChecksUI();

            return services;
        }

        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
        {
            var options = new HealthCheckOptions
            {
                AllowCachingResponses = false,
                ResponseWriter = async (context, report) =>
                {
                    var result = JsonConvert.SerializeObject(
                        new
                        {
                            status = report.Status.ToString(),
                            errors = report.Entries.Select(e =>
                                new
                                {
                                    key = e.Key,
                                    value = Enum.GetName(typeof(HealthStatus),
                                    e.Value.Status)
                                })
                        });
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
                }
            };

            options.ResultStatusCodes[HealthStatus.Unhealthy] = 500;
            options.ResultStatusCodes[HealthStatus.Degraded] = 500;

            //Adicionando endpoint para Middleware checar a saúde do serviço
            app.UseHealthChecks("/health", options);

            return app;
        }

        public static IApplicationBuilder UseElmah(this IApplicationBuilder app)
        {
            app.UseHealthChecksUI();

            //app.UseElmahIo();

            return app;
        }
    }
}
