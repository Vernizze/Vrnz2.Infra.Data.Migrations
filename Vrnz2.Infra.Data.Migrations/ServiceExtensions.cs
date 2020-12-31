using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Vrnz2.Infra.Data.Migrations
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigSqLiteMigrations<T>(this IServiceCollection services, string databaseFullPath)
        {
            services
                .AddFluentMigratorCore()
                    .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString($"Data Source={databaseFullPath}")
                    .ScanIn(typeof(T).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            return services;
        }
    }
}
