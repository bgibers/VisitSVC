using System;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Visit.DataAccess.Auth;
using Visit.DataAccess.Auth.Helpers;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;
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
            services.AddControllers()   
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddOpenApiDocument();
            services.AddDbContext<VisitContext>(
                options => options.UseMySql(Configuration.GetConnectionString("MySql"),
                    mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(5, 7, 17),
                            ServerType.MySql); // replace with your Server Version and Type
                    }
                ).EnableSensitiveDataLogging());
            services.AddAutoMapper(typeof(Startup));

            // Map settings
            services.Configure<BlobConfig>(Configuration.GetSection("BlobStorageAcct"));

            // Authentication
            services.AddSingleton<IJwtFactory, JwtFactory>();
            
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var signingKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(jwtAppSettingOptions[nameof(JwtIssuerOptions.SigningKey)]));
            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey,
                    SecurityAlgorithms.HmacSha256);
            });
            
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("VisitUser", policy => 
                    policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            });
            
            var builder = services.AddIdentityCore<User>(o =>
            {
                o.User.RequireUniqueEmail = true;
                
                // configure identity options
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<VisitContext>().AddDefaultTokenProviders();

            
            // Services and BL
            services.AddScoped<PostTestDataService>();
            services.AddTransient<IBlobStorageBusinessLogic, BlobStorageBusinessLogic>();
            services.AddTransient<IAccountsService, AccountsService>();

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
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
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
                    else
                    {
                        await next();
                    }
                });
            });
        }
    }
}

// options.Authority = $"https://cognito-idp.{Configuration.GetSection("AWS")["Region"]}.amazonaws.com/{Configuration.GetSection("AwsCognito")["UserPoolId"]}";