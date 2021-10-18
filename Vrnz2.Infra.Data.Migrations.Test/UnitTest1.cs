using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Vrnz2.Infra.Data.Migrations.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var services = new ServiceCollection();

            services.AddSeeding("mongodb://me-scheduler:user123@localhost:27020/me-scheduler?w=majority", "me-scheduler");
        }
    }
}
