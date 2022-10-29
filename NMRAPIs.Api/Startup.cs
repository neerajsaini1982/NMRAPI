// <copyright file="Startup.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.Api
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using FluentValidation.AspNetCore;
    using MediatR;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Rewrite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using NMRAPIs.Api.AuthExtensions;
    using NMRAPIs.BusinessLogic.Features.Profile;
    using NMRAPIs.BusinessLogic.Helpers;
    using NMRAPIs.BusinessLogic.Middleware;
    using NMRAPIs.BusinessLogic.ServicesInterfaces;
    using NMRAPIs.Core.Constants;
    using NMRAPIs.Core.ResponseMiddleware;
    using NMRAPIs.Infrastructure;
    using NMRAPIs.Infrastructure.Context;
    using NMRAPIs.Infrastructure.OracleContext;
    using NMRAPIs.Infrastructure.ServiceImplementation;
    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>
    /// .Net Core Startup Class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">configuration.</param>
        /// <param name="environment">environment.</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment = null)
        {
            this.Configuration = configuration;
            this.HostingEnvironment = environment;
            CommonSettings.AppSettings = this.Configuration.GetSection("AppSettings").Get<ApplicationSettings>();
        }

        /// <summary>
        /// Gets hostingEnvironment.
        /// </summary>
        public IWebHostEnvironment HostingEnvironment { get; }


        /// <summary>
        /// Gets the Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">IServiceCollection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddAzureAdBearer(options => Configuration.Bind("AzureAd", options));

            // DB Context.
            services.AddDbContext<NMRAPIsContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("Default")));

            services.AddDbContext<OracleDBContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("OracleConnection")));

            // Add MediatR.
            services.AddMediatR(typeof(GetAllUsersCommand));

            // Add MediatR Authorization Pipeline Handlers
            services.RegisterAuthorizationHandlers(new[] { typeof(GetAllUsersCommand).Assembly }, ServiceLifetime.Scoped);

            // Register IHttpContextAccessor for services to get access to the HttpContext.
            services.AddHttpContextAccessor();

            // Register All Services
            services.RegisterAllTypesWithBaseInterface<IBaseService>(new[] { typeof(ProfileService).Assembly }, ServiceLifetime.Scoped);

            // Add service and create Policy with options.
            List<string> allowedCorsOriginsEndPoints = $"{this.Configuration["CSRFDomains"]}".Split('|', System.StringSplitOptions.RemoveEmptyEntries).ToList();
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins(allowedCorsOriginsEndPoints.ToArray())
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert NMRAPIs Reference with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NMREvennts Api", Version = "v1" });
                //c.OperationFilter<SwaggerCSRFParameter>();

                // Locate the XML file being generated by ASP.NET...
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);

                c.AddFluentValidationRulesScoped();
            });

            // Add AutoMapper Profile
            services.AddAutoMapper(typeof(AutoMapping));

            // Add MediatR Pipelines.
            services.AddMediatRPipelines();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest).AddFluentValidation(cfg =>
            {
                cfg.RegisterValidatorsFromAssemblyContaining(typeof(GetAllUsersCommand));
                cfg.ImplicitlyValidateChildProperties = true;
                //cfg.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                cfg.DisableDataAnnotationsValidation = true;
            });

            services.AddOptions();

            services.Configure<AzureAdSettings>(Configuration.GetSection("AzureAd"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // global policy
            app.UseCors("CorsPolicy");

            app.UseRequestResponseLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseApiResponseWrapperMiddleware();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //   Path.Combine(Directory.GetCurrentDirectory(), @"resources")),
            //    RequestPath = new PathString("/resources"),
            //    ServeUnknownFileTypes = true
            //});
        }
    }
}
