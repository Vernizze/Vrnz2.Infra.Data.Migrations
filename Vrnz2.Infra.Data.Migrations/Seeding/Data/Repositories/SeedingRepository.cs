using System.Threading.Tasks;
using Vrnz2.Infra.CrossCutting.Extensions;
using Vrnz2.Infra.Repository.Abstract;
using Entity = Vrnz2.Infra.Data.Migrations.Seeding.Data.Entities;

namespace Vrnz2.Infra.Data.Migrations.Seeding.Data.Repositories
{
    public class SeedingRepository
        : BaseRepository, ISeedingRepository
    {
        #region Constructors

        public SeedingRepository()
            => TableName = nameof(Entity.Seeding);

        #endregion

        #region Methods

        public async Task<bool> InsertAsync(Entity.Seeding value)
            => await InsertAsync(value);

        public async Task<bool> SeedingDoneAsync(string seedingNumber)
        {
            var seed = await QueryFirstOrDefaultAsync<Entity.Seeding>("SELECT * FROM Seeds WHERE Number = @seedingNumber;", new { seedingNumber });

            return seed.IsNotNull();
        }

        #endregion
    }
}
