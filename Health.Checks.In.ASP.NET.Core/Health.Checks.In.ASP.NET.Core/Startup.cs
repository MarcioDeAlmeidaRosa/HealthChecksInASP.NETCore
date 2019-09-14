﻿using Health.Checks.In.ASP.NET.Core.Infrastructure.Data.Model;
using Health.Checks.In.ASP.NET.Core.Infrastructure.HealthChecks.HealthCheck;
using Health.Checks.In.ASP.NET.Core.Infrastructure.HealthChecks.Services;
using Health.Checks.In.ASP.NET.Core.Repositories.Interface;
using Health.Checks.In.ASP.NET.Core.Repositories.Repository;
using Health.Checks.In.ASP.NET.Core.Services.Interfaces;
using Health.Checks.In.ASP.NET.Core.Services.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Health.Checks.In.ASP.NET.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<Settings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            services.Configure<MemoryCheckOptions>(name: "MemoryCheckOptions",  options =>
            {
                options.Threshold = Convert.ToInt64(Configuration.GetSection("MemoryCheckOptions:Threshold").Value);
            });

            services.AddTransient<IVehicleService, VehicleService>();
            services.AddTransient<IVehicleRepository, VehicleRepository>();

            //Adicionando Middleware para checar a saúde do serviço
            services.AddSingleton<StartupHostedServiceHealthCheck>();
            services.AddHostedService<StartupHostedService>();
            services.AddHealthChecks().AddMemoryHealthCheck("MemoryCheckOptions");
            services.AddHealthChecks().AddCheck<StartupHostedServiceHealthCheck>("slow_dependency_check");
            //---fim
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            //Adicionando endpoint para Middleware checar a saúde do serviço
            app.UseHealthChecks("/health");
        }
    }
}
