using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dropbox.Application.Common;
using Dropbox.Application.Common.Interfaces;
using Dropbox.Domain.Entities;
using Dropbox.Infrastructure.Persistence;
using System;
using System.Reflection;

namespace Dropbox.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string contentRootPath, bool isContextTransient = false)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddDbContext<ApplicationDbContext>(options =>
                options
                    .UseNpgsql(
                        configuration.GetConnectionString("DefaultConnection"),
                        b =>
                        {
                            b.EnableRetryOnFailure()
                             .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        }).EnableSensitiveDataLogging(), isContextTransient ? ServiceLifetime.Transient : ServiceLifetime.Scoped
            );

            services.AddMemoryCache();
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            services.Configure<DataProtectionTokenProviderOptions>(options => {
                options.TokenLifespan = TimeSpan.FromDays(7);
            });

            return services;
        }

    }
}