using Bogus;
using Base.Api.Domain.Models;
using Base.Common.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infrastructure.Persistance.Context;

internal class SeedData
{
    private static List<User> GetUsers()
    {
        var result = new Faker<User>("tr")
            .RuleFor(x => x.Id, x => Guid.NewGuid())
            .RuleFor(x => x.CreateDate,
                    x => x.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
            .RuleFor(x => x.Username, x => x.Person.UserName)
            .RuleFor(x => x.EmailAddress, x => x.Internet.Email())
            .RuleFor(x => x.Password, x => PasswordEncryptor.Encrypt(x.Internet.Password()))
            .RuleFor(x => x.EmailConfirmed, x => x.PickRandom(true, false))
            .RuleFor(x => x.IsActive, x => x.PickRandom(true, false))
        .Generate(500);
    
        return result;
    }


    public async Task SeedAsync(IConfiguration configuration)
    {
        var dbContextBuilder = new DbContextOptionsBuilder();
        dbContextBuilder.UseSqlServer(configuration.GetConnectionString("BaseConnectionString"));

        var context = new BaseContext(dbContextBuilder.Options);

        // Continue if there isn't any data in database.
        if (context.Users.Any())
        {
            await Task.CompletedTask;
            return;
        }

        // Get mock users and add them to database.
        var users = GetUsers();
        var userIds = users.Select(x => x.Id);

        await context.Users.AddRangeAsync(users);

        // todo Get more mock datas and add them to database.
        //
        //      do something...
        //
        // *****************************************

        await context.SaveChangesAsync();
    }

}
