using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
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

        public void Run(string connectionString, string database)
        {
            var path = Path.Combine(FilesAndFolders.AppPath(), "seedings", "mongodb", "seeds");

            if (Directory.Exists(path))
            {
                var inserted = true;

                _unitOfWork.OpenConnection();

                _unitOfWork.Begin();

                var repository = _unitOfWork.GetRepository<ISeedsRepository>(nameof(Entity.Seed));

                try
                {
                    Directory.GetFiles(path, "*.json").SForEach(m =>
                    {
                        var seeds = JsonConvert.DeserializeObject<SeedingsFileContent>(FilesAndFolders.GetFileContent(m));

                        var seedingNumber = Path.GetFileName(m).Replace(".json", string.Empty);

                        if (!(repository.SeedingDoneAsync(seedingNumber).GetAwaiter().GetResult()))
                        {
                            seeds.Seeds.SForEach(seed =>
                            {
                                using (var mongo = new Mongo(connectionString, seed.CollectionName, database))
                                {
                                    seed.Values.SForEach(i =>
                                    {
                                        dynamic expando = new ExpandoObject();

                                        AddProperty(expando, i.Values);

                                        mongo.Add(expando).GetAwaiter().GetResult();
                                    });
                                }
                            });

                            inserted = inserted && repository.Insert(new Entity.Seed { Id = Guid.NewGuid().ToString(), CreationDateTime = DateTime.UtcNow, Number = seedingNumber });
                        }
                    });
                }
                catch (Exception ex)
                {
                    _unitOfWork.Rollback();
                    _unitOfWork.Dispose();

                    throw;
                }

                if (inserted)
                    _unitOfWork.Commit();
                else
                    _unitOfWork.Rollback();

                _unitOfWork.Dispose();
            }
        }

        public ExpandoObject AddProperty(ExpandoObject expando, List<SeedValue> values)
        {
            values.SForEach(v => 
            {
                var expandoDict = expando as IDictionary<string, object>;
                if (expandoDict.ContainsKey(v.PropertyName))
                    expandoDict[v.PropertyName] = v.Value;
                else
                    expandoDict.Add(v.PropertyName, v.Value);
            });

            return expando;
        }

        #endregion
    }
}
