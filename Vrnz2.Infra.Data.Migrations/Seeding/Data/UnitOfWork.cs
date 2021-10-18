using Microsoft.Data.Sqlite;
using System.Data;
using System.IO;
using System.Linq;
using Vrnz2.Infra.Data.Migrations.Seeding.Data.Repositories;
using Vrnz2.Infra.Repository.Abstract;

namespace Vrnz2.Infra.Data.Migrations.Seeding.Data
{
    public class UnitOfWork
        : BaseUnitOfWork
    {
        #region Constants

        public const string DbName = "SeedsDB.db";

        #endregion

        #region Constructors

        public UnitOfWork()
        {
            _connection = new SqliteConnection(GetConnectionString(string.Empty));

            AddRepository<ISeedsRepository, SeedsRepository>();
        }

        public UnitOfWork(string basePath)
        {
            _connection = new SqliteConnection(GetConnectionString(basePath));

            AddRepository<ISeedsRepository, SeedsRepository>();
        }

        #endregion

        #region Methods

        public static string GetConnectionString(string dbFilePath)
            => string.Concat("Data Source=", Path.Combine(dbFilePath, DbName));

        public override void InitReps(IDbConnection dbConnection)
            => _repositories.ToList().ForEach(r => r.Value.Init(dbConnection));

        protected override void InitReps(IDbTransaction dbTransaction)
            => _repositories.ToList().ForEach(r => r.Value.Init(dbTransaction));

        #endregion
    }
}
