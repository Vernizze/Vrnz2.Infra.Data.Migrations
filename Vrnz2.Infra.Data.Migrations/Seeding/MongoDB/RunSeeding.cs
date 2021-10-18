using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Vrnz2.Infra.CrossCutting.Extensions;
using Vrnz2.Infra.CrossCutting.Utils;
using Vrnz2.Infra.Data.Migrations.Seeding.Data.DTO;
using Vrnz2.Infra.Data.Migrations.Seeding.Data.Repositories;
using Vrnz2.Infra.Repository.Interfaces.Base;
using Entity = Vrnz2.Infra.Data.Migrations.Seeding.Data.Entities;
using Mongo = Vrnz2.Data.MongoDB.MongoDB;

namespace Vrnz2.Infra.Data.Migrations.Seeding.MongoDB
{
    public class RunSeeding
    {
        #region Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        public RunSeeding(IUnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;

        #endregion        

        #region Methods

        public Task Run(string connectionString, string database)
        {
            var path = Path.Combine(FilesAndFolders.AppPath(), "migrations", "mongodb", "seeding");

            if (Directory.Exists(path))
            {
                _unitOfWork.OpenConnection();

                var repository = _unitOfWork.GetRepository<ISeedingRepository>(nameof(Entity.Seeding));

                Directory.GetFiles(path, "*.json").SForEach(async m =>
                {
                    var seeds = JsonConvert.DeserializeObject<SeedingsFileContent>(FilesAndFolders.GetFileContent(m));

                    if (!(await repository.SeedingDoneAsync(Path.GetFileName(m).Replace(".json", string.Empty))))
                    {
                        seeds.Seeds.SForEach(async seed => 
                        {
                            using (var mongo = new Mongo(connectionString, seed.CollectionName, database))
                            {
                                //string json = @"{""key1"":""value1"",""key2"":""value2""}";

                                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(seed.Values);

                                dynamic expando = new ExpandoObject();

                                values.SForEach(i => AddProperty(expando, i.Key, i.Value));

                                await mongo.Add(expando);
                            }
                        });                        
                    }
                });

                _unitOfWork.Dispose();
            }

            return Task.CompletedTask;
        }

        public void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject da suporte a IDictionary então podemos estendê-lo assim:
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        #endregion
    }
}
