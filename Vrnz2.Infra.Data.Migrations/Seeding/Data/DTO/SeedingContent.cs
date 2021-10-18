using System.Collections.Generic;

namespace Vrnz2.Infra.Data.Migrations.Seeding.Data.DTO
{
    public class SeedingContent
    {
        public string CollectionName { get; set; }
        public List<SeedValues> Values { get; set; }
    }
}
