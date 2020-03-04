using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Visit.DataAccess.EntityFramework;
using Visit.Service.BusinessLogic;
using Visit.Service.BusinessLogic.BlobStorage;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Config;

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
            services.AddControllers();
            services.AddOpenApiDocument();
            services.AddDbContext<VisitContext>( 
                options => options.UseMySql(Configuration.GetConnectionString("MySql"), 
                    mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(5, 7, 17), ServerType.MySql);// replace with your Server Version and Type
                    }
                ).EnableSensitiveDataLogging());
            services.AddAutoMapper(typeof(Startup));
            
            // Map settings
            services.Configure<BlobConfig>(Configuration.GetSection("BlobStorageAcct"));
            
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.ClaimsIssuer = "VisitBackend";
                    options.Audience = Configuration.GetSection("AWS")["UserPoolClientId"];
                    options.Authority = $"https://cognito-idp.{Configuration.GetSection("AWS")["Region"]}.amazonaws.com/{Configuration.GetSection("AwsCognito")["UserPoolId"]}";
                });

            // Services and BL
            services.AddScoped<PostTestDataService>();
            services.AddTransient<IBlobStorageBusinessLogic, BlobStorageBusinessLogic>();
            services.AddTransient<IAccountsBusinessLogic, AccountsBusinessLogic>();

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
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseOpenApi();
            app.UseSwaggerUi3();
            
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

                    //when authorization has failed, should retrun a json message to client
                    if (error != null && error.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            State = "Unauthorized",
                            Msg = "Token expired"
                        }));
                    }
                    //when orther error, retrun a error message json to client
                    else if (error != null && error.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            State = "Internal Server Error",
                            Msg = error.Error.Message
                        }));
                    }
                    //when no error, do next.
                    else await next();
                });
            });
        }
    }
}