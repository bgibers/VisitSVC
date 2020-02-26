using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using AutoMapper;
using Visit.DataAccess;
using Visit.Service.BusinessLogic;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.ConfigModels;
using Visit.Service.Services;

namespace Visit.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Config, DB, and swagger
            services.AddSingleton(Configuration);
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddOpenApiDocument();
            services.AddDbContext<VisitContext>( 
                options => options.UseMySql(Configuration.GetConnectionString("MySql"), 
                    mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(5, 7, 17), ServerType.MySql);// replace with your Server Version and Type
                    }
                ).EnableSensitiveDataLogging());
            services.AddAutoMapper(typeof(Startup));

            // Services and BL
            services.AddScoped<PostTestDataService>();
            services.AddTransient<IBlobStorageBusinessLogic, BlobStorageBusinessLogic>();

            // Cors policy
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(host => true)
                );
            });
            
            services.Configure<BlobConfig>(Configuration.GetSection("BlobStorageAcct"));

        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}