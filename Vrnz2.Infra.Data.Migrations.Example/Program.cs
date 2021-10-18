using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Vrnz2.Infra.Data.Migrations.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            var connectionString = "mongodb://me-scheduler:user123@localhost:27020/me-scheduler?w=majority";

            services
                .AddSeeding(Assembly.GetAssembly(typeof(Program)), connectionString, "me-scheduler");

            Console.WriteLine("Processo finalizado com sucesso...");

            Console.ReadLine();
        }
    }
}
