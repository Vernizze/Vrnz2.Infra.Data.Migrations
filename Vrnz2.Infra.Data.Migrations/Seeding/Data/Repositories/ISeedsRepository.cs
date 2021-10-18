using System.Threading.Tasks;
using Vrnz2.Infra.Repository.Interfaces.Base;
using Entity = Vrnz2.Infra.Data.Migrations.Seeding.Data.Entities;

namespace Vrnz2.Infra.Data.Migrations.Seeding.Data.Repositories
{
    public interface ISeedsRepository
        : IBaseRepository
    {
        Task<bool> SeedingDoneAsync(string seedingNumber);
        bool Insert(Entity.Seed value);
    }
}
