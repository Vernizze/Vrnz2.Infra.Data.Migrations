using System.Threading.Tasks;
using Vrnz2.Infra.Repository.Interfaces.Base;
using Entity = Vrnz2.Infra.Data.Migrations.Seeding.Data.Entities;

namespace Vrnz2.Infra.Data.Migrations.Seeding.Data.Repositories
{
    public interface ISeedingRepository
        : IBaseRepository
    {
        Task<bool> SeedingDoneAsync(string seedingNumber);
        Task<bool> InsertAsync(Entity.Seeding value);        
    }
}
