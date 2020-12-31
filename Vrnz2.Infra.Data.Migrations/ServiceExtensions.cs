using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Vrnz2.Infra.Data.Migrations
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigSqLiteMigrations(this IServiceCollection services, Assembly assembly, string databaseFullPath)
        {
            services
                .AddFluentMigratorCore()
                    .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString($"Data Source={databaseFullPath}")
                    .ScanIn(assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            return services;
        }
    }
}
