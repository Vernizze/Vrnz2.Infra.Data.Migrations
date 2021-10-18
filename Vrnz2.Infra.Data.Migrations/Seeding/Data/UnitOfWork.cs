using Microsoft.Data.Sqlite;
using System.Data;
using System.Linq;
using Vrnz2.Infra.Data.Migrations.Seeding.Data.Repositories;
using Vrnz2.Infra.Repository.Abstract;

namespace Vrnz2.Infra.Data.Migrations.Seeding.Data
{
    public class UnitOfWork
        : BaseUnitOfWork
    {
        #region Constants

        public const string ConnectionString = "Data Source=SeedindDB.db";

        #endregion

        #region Constructors

        public UnitOfWork()
        {
            _connection = new SqliteConnection(ConnectionString);

            AddRepository<ISeedingRepository, SeedingRepository>();
        }

        #endregion

        public override void InitReps(IDbConnection dbConnection)
            => _repositories.ToList().ForEach(r => r.Value.Init(dbConnection));

        protected override void InitReps(IDbTransaction dbTransaction)
            => _repositories.ToList().ForEach(r => r.Value.Init(dbTransaction));
    }
}
