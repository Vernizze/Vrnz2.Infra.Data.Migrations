using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace Vrnz2.Infra.Data.Migrations
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigSqLiteMigrations(this IServiceCollection services, Assembly assembly, string databasePath, string databaseName)
        {
            if (!Directory.Exists(databasePath))
                Directory.CreateDirectory(databasePath);

            services
                .AddFluentMigratorCore()
                    .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString($"Data Source={Path.Combine(databasePath, databaseName)}")
                    .ScanIn(assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            using (var scope = services.BuildServiceProvider().CreateScope())
                UpdateDatabase(scope.ServiceProvider);

            return services;
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
            => serviceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
    }
}
