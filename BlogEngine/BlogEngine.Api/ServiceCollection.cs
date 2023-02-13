using System;
using System.IO;
using System.Reflection;
using BlogEngine.Core.Data.DatabaseContexts;
using BlogEngine.Core.Services.Abstractions;
using BlogEngine.Core.Services.Implementations;
using BlogEngine.Api.Services.Abstractions;
using BlogEngine.Api.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using AutoMapper;
using BlogEngine.Core.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlogEngine.Core;
using BlogEngine.Api.Services.Abstractions.Identity;
using BlogEngine.Api.Services.Implementations.Identity;

namespace BlogEngine.Api
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddServerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCoreServices(configuration);

            services.AddIdentity();

            services.AddUserProviders();

            services.AddJWTAuthentication(configuration);

            services.AddJWTServices();

            services.AddApplicationServices();

            services.AddJson();

            services.AddMapper();

            services.AddSwagger();

            return services;
        }

        private static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole<int>>(config =>
            {
                // Temporary simple validation
                config.Password.RequiredLength = 1;
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();
        }

        private static void AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var JWTKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,

                        IssuerSigningKey = new SymmetricSecurityKey(JWTKey),
                    };
                });
        }

        private static void AddJWTServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, JWTTokenService>();
        }

        private static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IReadingTimeEstimator, ReadingTimeEstimator>();

            services.AddScoped<IPostService, PostService>();

            services.AddScoped<ICommentService, CommentService>();

            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<IRoleManager, RoleManager>();

        }

        private static void AddJson(this IServiceCollection services)
        {
            services.AddMvc()
               .AddNewtonsoftJson(jsonOptions =>
               {
                   jsonOptions.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
               });
        }

        private static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(configuration =>
            {
                configuration.AllowNullCollections = true;
                configuration.AllowNullDestinationValues = true;
            }, typeof(Startup));
        }

        private static void AddUserProviders(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        }

        private static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                var openApiInfo = new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "BlogEngine.API",
                    Description = "This is a Web API for BlogEngine project",
                };

                config.SwaggerDoc("v1", openApiInfo);

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                config.IncludeXmlComments(xmlPath);
            });
        }
    }
}