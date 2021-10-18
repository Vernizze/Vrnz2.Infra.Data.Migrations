using System.Collections.Generic;

namespace Vrnz2.Infra.Data.Migrations.Seeding.Data.DTO
{
    public class SeedingsFileContent
    {
        public string Number { get; set; }
        public List<SeedingContent> Seeds { get; set; }
    }
}
