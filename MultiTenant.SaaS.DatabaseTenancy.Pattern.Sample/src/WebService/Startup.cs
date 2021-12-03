using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kledex;
using Kledex.Commands;
using Kledex.Extensions;
using Kledex.Validation.FluentValidation.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.CommandHandlers;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Commands;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Domain.Queries;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.Infrastructure.DBContexts;
using MultiTenant.SaaS.DatabaseTenancy.Pattern.Sample.WebService.Controllers;
using Newtonsoft.Json;

namespace WebService
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
            //services.Configure<AppSettings>(Configuration.GetSection(AppSettings.AppSettingsKey));

            services
            .AddKledex(options =>
            {
                options.PublishEvents = false;
                options.SaveCommandData = false;
            },

                typeof(RegisterUserCommand),
                typeof(SubscribeCommand),
                typeof(CancelSubscriptionCommand),
                typeof(UsersExistenceQuery)
            )
            .AddFluentValidation(options =>
                {
                    options.ValidateAllCommands = true;
                }
            );


            services.AddDbContext<UserManagementDbContext>(options => {
                options.UseCosmos(
                    accountEndpoint: this.Configuration.GetValue<string>("Cosmos:AccountEndpoint"),
                    accountKey: this.Configuration.GetValue<string>("Cosmos:AccountKey"),
                    databaseName: this.Configuration.GetValue<string>("Cosmos:UMDatabaseName")
                );
            });

            services.AddDbContext<SharedDbContext>(options => {
                options.UseCosmos(
                    accountEndpoint: this.Configuration.GetValue<string>("Cosmos:AccountEndpoint"),
                    accountKey: this.Configuration.GetValue<string>("Cosmos:AccountKey"),
                    databaseName: this.Configuration.GetValue<string>("Cosmos:SharedDatabaseName")
                );
            });
            
            services.AddDbContext<TenantBasedDynamicDbContext>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
