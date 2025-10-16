using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EventManagement.EntityFrameworkCore
{
    // تعليق: مصنع DbContext للـ EF Core Tools لتوليد الـ Migrations
    public class EventManagementMigrationsDbContextFactory : IDesignTimeDbContextFactory<EventManagementMigrationsDbContext>
    {
        public EventManagementMigrationsDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<EventManagementMigrationsDbContext>()
                .UseNpgsql(GetConnectionStringFromAppSettings());

            return new EventManagementMigrationsDbContext(builder.Options);
        }

        private static string GetConnectionStringFromAppSettings()
        {
            var basePath = Directory.GetCurrentDirectory();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(Path.Combine(basePath, "..", "EventManagement.DbMigrator", "appsettings.json"), optional: false)
                .AddJsonFile(Path.Combine(basePath, "..", "EventManagement.DbMigrator", "appsettings.secrets.json"), optional: true)
                .Build();

            return configuration.GetConnectionString("Default");
        }
    }
}


