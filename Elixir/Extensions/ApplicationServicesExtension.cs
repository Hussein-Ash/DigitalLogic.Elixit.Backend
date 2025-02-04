using System.Globalization;
using e_parliament.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.S3;
using Elixir.DATA;
using Elixir.Helpers;
using Elixir.Repository;
using Elixir.Services;
using Elixir.Helpers.OneSignal;
using Elixir.Services.StaticService;
using Microsoft.AspNetCore.Authorization;
using Elixir.Extensions.StoreAuthoeization;

namespace Elixir.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddAWSService<IAmazonS3>();

            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
            // services.AddScoped<AuthorizeActionFilter>();


            // services.AddScoped<PermissionSeeder>();

            // seed data from permission seeder service

            // Register the authorization handler
            services.AddSingleton<IAuthorizationHandler, StoreIdAuthorizationHandler>();

            // Register the authorization policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("StoreIdPolicy", policy =>
                    policy.Requirements.Add(new StoreIdRequirement()));
            });

            services.AddHttpContextAccessor();

            services.Scan(scan => scan
                .FromAssemblyOf<Program>()
                .AddClasses()
                .AsMatchingInterface()
                .WithScopedLifetime()
            );


            var serviceProvider = services.BuildServiceProvider();
            // var permissionSeeder = serviceProvider.GetService<PermissionSeeder>();
            // permissionSeeder.SeedPermissions();

            return services;
        }
    }
}