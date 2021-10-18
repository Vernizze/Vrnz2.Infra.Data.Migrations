using System.Threading.Tasks;
using Vrnz2.Infra.CrossCutting.Extensions;
using Vrnz2.Infra.Repository.Abstract;
using Entity = Vrnz2.Infra.Data.Migrations.Seeding.Data.Entities;

namespace Vrnz2.Infra.Data.Migrations.Seeding.Data.Repositories
{
    public class SeedsRepository
        : BaseRepository, ISeedsRepository
    {
        #region Constructors

        public SeedsRepository()
            => TableName = nameof(Entity.Seed);

        #endregion

        #region Methods

        public bool Insert(Entity.Seed value)
            => Execute("INSERT INTO Seed (Id, CreationDateTime, Number) VALUES (@Id, @CreationDateTime, @Number)", new { value.Id, value.CreationDateTime, value.Number }) > 0;
        
        public async Task<bool> SeedingDoneAsync(string seedingNumber)
        {
            var seed = await QueryFirstOrDefaultAsync<Entity.Seed>("SELECT Id, CreationDateTime, Number FROM Seed WHERE Number = @seedingNumber;", new { seedingNumber });

            return seed.IsNotNull();
        }

        #endregion
    }
}
