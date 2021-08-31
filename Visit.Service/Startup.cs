﻿using System;
 using System.IO;
 using AutoMapper;
 using FirebaseAdmin;
 using FirebaseAdmin.Auth;
 using FirebaseAdmin.Messaging;
 using Google.Apis.Auth.OAuth2;
 using Microsoft.AspNetCore.Authentication.JwtBearer;
 using Microsoft.AspNetCore.Builder;
 using Microsoft.AspNetCore.Diagnostics;
 using Microsoft.AspNetCore.Hosting;
 using Microsoft.AspNetCore.Http;
 using Microsoft.EntityFrameworkCore;
 using Microsoft.Extensions.Configuration;
 using Microsoft.Extensions.DependencyInjection;
 using Microsoft.Extensions.Hosting;
 using Microsoft.IdentityModel.Tokens;
 using Microsoft.OpenApi.Models;
 using Newtonsoft.Json;
 using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
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
             services.AddControllers()
                 .AddNewtonsoftJson(options =>
                     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                 );
             services.AddSwaggerGen(c =>
             {
                 c.SwaggerDoc("v1", new OpenApiInfo {Title = "Visit", Version = "v1"});
                 c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                 {
                     Name = "Authorization",
                     Type = SecuritySchemeType.ApiKey,
                     Scheme = "Bearer",
                     BearerFormat = "JWT",
                     In = ParameterLocation.Header,
                     Description = ""
                 });

                 c.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                     {
                         new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = "Bearer"
                             }
                         },
                         new string[] { }
                     }
                 });

             });
             services.AddDbContext<VisitContext>(
                 options => options.UseMySql(Configuration.GetConnectionString("MySql"),
                     mySqlOptions =>
                     {
                         mySqlOptions.ServerVersion(new Version(5, 7, 17),
                             ServerType.MySql); // replace with your Server Version and Type

                         mySqlOptions.MigrationsAssembly("Visit.Service");
                     }
                 ).EnableSensitiveDataLogging());
             services.AddAutoMapper(typeof(Startup));

             // Map settings
             services.Configure<BlobConfig>(Configuration.GetSection("BlobStorageAcct"));

             // Authentication
             services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     var projectId = Configuration.GetSection("Firebase:project_id").Value;
                     
                     options.Authority = $"https://securetoken.google.com/{projectId}";
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidIssuer = $"https://securetoken.google.com/{projectId}",
                         ValidateAudience = true,
                         ValidAudience = projectId,
                         ValidateLifetime = true
                     };
                 });

             
             // Services and BL
             services.AddScoped<PostTestDataService>();
             services.AddTransient<IBlobStorageBusinessLogic, BlobStorageBusinessLogic>();
             services.AddTransient<IUserBusinessLogic, UserBusinessLogic>();
             services.AddTransient<IAccountsService, AccountsService>();
             services.AddTransient<IPostService, PostService>();
             services.AddTransient<IDevopsService, DevopsService>();
             services.AddTransient<IFirebaseService, FirebaseService>();

             if (!Configuration["APPINSIGHTS_CONNECTIONSTRING"].Equals(""))
             {
                 services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
             }
             
             AddFireBase(services, Configuration);

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

             app.UseSwagger();
             app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Visit API V1"); });
             app.UseRouting();
             app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
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

         private static void AddFireBase(IServiceCollection services, IConfiguration configuration)
         {
             var path = Path.Combine(Directory.GetCurrentDirectory(), "Secrets", configuration.GetSection("Firebase:fileName").Value);
             FirebaseApp app;
             try
             {
                 app = FirebaseApp.Create(new AppOptions
                 {
                     Credential = GoogleCredential.FromFile(path)
                 }, "FBApp");
             }
             catch (Exception)
             {
                 app = FirebaseApp.GetInstance("FBApp");
             }

             services.AddSingleton(FirebaseMessaging.GetMessaging(app));
             services.AddSingleton(FirebaseAuth.GetAuth(app));
         }
     }
 }
 