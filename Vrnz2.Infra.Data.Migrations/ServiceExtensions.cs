using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Reflection;

namespace Vrnz2.Infra.Data.Migrations
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigSqLiteMigrations(this IServiceCollection services, Assembly assembly, string connectionString)
        {
            services
                .AddFluentMigratorCore()
                    .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            using (var scope = services.BuildServiceProvider().CreateScope())
                UpdateDatabase(scope.ServiceProvider);

            return services;
        }

        public static IServiceCollection ConfigPostgresMigrations(this IServiceCollection services, Assembly assembly, string connectionString)
        {
            services
                .AddFluentMigratorCore()
                    .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            using (var scope = services.BuildServiceProvider().CreateScope())
                UpdateDatabase(scope.ServiceProvider);

            return services;
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
            => serviceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();

        public static IServiceCollection CreatePostgresDatabase(this IServiceCollection services, string databaseName, string ownerConnectionString)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ownerConnectionString))
            {
                conn.Open();

                if (!DbExists(databaseName, conn))
                    Create(databaseName, conn);

                conn.Close();
            }

            return services;
        }

        private static bool Create(string dbname, NpgsqlConnection conn)
        {
            string sql = $"CREATE DATABASE {dbname} WITH OWNER = postgres ENCODING = 'UTF8' CONNECTION LIMIT = -1;";

            using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
            {
                try
                {
                    var result = command.ExecuteScalar();

                    return (result != null && result.ToString().Equals(dbname));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());

                    throw;
                }
            }
        }

        private static bool DbExists(string dbname, NpgsqlConnection conn)
        {
            string sql = $"SELECT DATNAME FROM pg_catalog.pg_database WHERE DATNAME = '{dbname}'";

            using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
            {
                try
                {
                    var result = command.ExecuteScalar();

                    return (result != null && result.ToString().Equals(dbname));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());

                    throw;
                }
            }
        }
    }
}
