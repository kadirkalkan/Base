using Base.Api.Domain.Entity;
using Base.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infrastructure.Persistance.Context;

public class BaseContext : DbContext
{
    public const string DEFAULT_SCHEMA = "dbo";

    public BaseContext()
    {
        Database.Migrate();
    }

    public BaseContext(DbContextOptions options) : base(options)
    {
        Database.Migrate();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<EmailConfirmation> EmailConfirmations { get; set; }

    // It designed for Migration Creation phase. If the app would run as usual options would came from IoC
    // but right here we'll use parameterless constructor so we need this OnConfiguring method
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "Data Source =.; Initial Catalog=Base; Integrated Security=true";
            optionsBuilder.UseSqlServer(connectionString, opt =>
            {
                opt.EnableRetryOnFailure();
            });
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override int SaveChanges()
    {
        OnBeforeSave();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSave();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        OnBeforeSave();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSave();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void OnBeforeSave()
    {
        var addedEntities = ChangeTracker.Entries()
                                .Where(e => e.State == EntityState.Added)
                                .Select(e => (BaseEntity)e.Entity);
        
        PrepareAddedEntities(addedEntities);
    }

    private void PrepareAddedEntities(IEnumerable<BaseEntity> entities)
    {
        entities.ToList().ForEach(e => e.CreateDate = DateTime.Now);
    }
}
