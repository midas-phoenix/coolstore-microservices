using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure;
using NetCoreKit.Infrastructure.EfCore.Db;
using NetCoreKit.Infrastructure.EfCore.MySql;

namespace VND.CoolStore.Services.Inventory.v1.Db
{
    public class InventoryDbContext : AppDbContext
    {
        public InventoryDbContext(DbContextOptions options, IConfiguration config, IDomainEventDispatcher eventBus = null)
            : base(options, config, eventBus)
        {
        }
    }

    public class InventoryDbContextDesignFactory : IDesignTimeDbContextFactory<InventoryDbContext>
    {
        public InventoryDbContext CreateDbContext(string[] args)
        {
            var dbConnFactory = new DatabaseConnectionStringFactory();
            var conn = dbConnFactory.Create();
            var optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>()
                .UseMySql(
                    conn, sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(GetType().Assembly.FullName);
                        sqlOptions.ServerVersion(dbConnFactory.DbOptions.DbInfo);
                        sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    }
                );

            return new InventoryDbContext(optionsBuilder.Options, ConfigurationHelper.GetConfiguration());
        }
    }
}
