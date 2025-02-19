using IdentityServer.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using ConsulConfig.Settings;

namespace IdentityServer
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
            var consulConfig = Configuration.GetSection("ConsulConfig").Get<ConsulConfiguration>()!;
            
            services.AddSingleton<IConsulClient>(provider => new ConsulClient(config =>
            {
                config.Address = new Uri(consulConfig.Address);
            }));
            
            services.ConfigurePersistence(Configuration);
            services.ConfigureIdentity();
            services.ConfigureJWT(Configuration);

            services.ConfigureMisscellaneousServices();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IdentityServer", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, IConsulClient consulClient)
        {
            var consulConfig = Configuration.GetSection("ConsulConfig").Get<ConsulConfiguration>()!;

            lifetime.ApplicationStarted.Register(() =>
            {
                var registration = new AgentServiceRegistration
                {
                    ID = consulConfig.ServiceId,
                    Name = consulConfig.ServiceName,
                    Address = consulConfig.ServiceAddress,
                    Port = consulConfig.ServicePort
                };

                consulClient.Agent.ServiceRegister(registration).Wait();
            });

            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(consulConfig.ServiceId).Wait();
            });
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityServer v1"));
            }

            app.UseCors("CorsPolicy");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

