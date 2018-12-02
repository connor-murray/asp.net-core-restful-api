using CityInfo.API.Contracts;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace CityInfo.API
{
    public class Startup
    {
        public static IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddMvc().AddMvcOptions(options =>
                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()));

#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif

            services.AddDbContext<CityInfoContext>(o =>
                o.UseSqlServer(Startup.Configuration["connectionStrings:cityInfoDBConnectionString"]));

            services.AddScoped<IRepositoryService, RepositoryService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CityInfoContext cityInfoContext)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }   
            else
            {
                app.UseExceptionHandler();
            }

            cityInfoContext.EnsureSeedDataForContext();

            app.UseStatusCodePages();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<CityEntity, CityDto>();
                cfg.CreateMap<CityEntity, CityWithPointOfInterestDto>();
                cfg.CreateMap<PointOfInterestEntity, PointOfInterestDto>();

                cfg.CreateMap<PointOfInterest, PointOfInterestEntity>();
            });

            app.UseMvc();
        }
    }
}
