using Base.Api.Application.Interfaces.Repositories;
using Base.Infrastructure.Persistance.Context;
using Base.Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infrastructure.Persistance.Extensions;

public static class Registration
{
    public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DatabaseContext
        services.AddDbContext<BaseContext>(conf =>
        {
            var connectionString = configuration.GetConnectionString("BaseConnectionString");
            conf.UseSqlServer(connectionString, opt =>
            {
                opt.EnableRetryOnFailure();
            });
        });

        // SeedData
        var seedData = new SeedData();
        seedData.SeedAsync(configuration).GetAwaiter().GetResult();


        // Add Repositories to Dependency Injection
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmailConfirmationRepository, EmailConfirmationRepository>();


        return services;
    }
}
