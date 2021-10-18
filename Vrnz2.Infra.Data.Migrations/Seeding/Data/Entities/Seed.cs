using System;
using Vrnz2.Infra.Repository.Abstract;

namespace Vrnz2.Infra.Data.Migrations.Seeding.Data.Entities
{
    public class Seed
        : BaseDataObject
    {
        public string Number { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
