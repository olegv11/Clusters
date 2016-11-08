using System.Collections.Generic;
using MongoDB.Driver;
using ClusterDomain;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace ClusterMongo
{
    public class MongoDBContext : DBContext
    {
        public MongoDBContext(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            Db = client.GetDatabase(databaseName);
            CollectionName = collectionName;
        }

        public void SaveDataSet(IDataSet dataSet)
        {
            var filter = Builders<IDataSet>.Filter
                              .Eq("Name", dataSet.Name);

            long count = Db.GetCollection<IDataSet>(CollectionName).Count(filter);
            
            if (count == 0)
            {
                Db.GetCollection<IDataSet>(CollectionName).InsertOneAsync(dataSet).Wait();
            }
            else if(count == 1)
            {
                //var update = Builders<IDataSet>.Update
                //    .Set("Data", dataSet.Data)
                //    .Set("CreationTime", dataSet.CreationTime);
                //Db.GetCollection<IDataSet>(CollectionName).FindOneAndUpdateAsync(filter, update).Wait();
                Db.GetCollection<IDataSet>(CollectionName).DeleteOneAsync(filter).Wait();
                Db.GetCollection<IDataSet>(CollectionName).InsertOneAsync(dataSet).Wait();
            }
            else
            {
                throw new MongoDBException("Найден повтор в базе данных");
            }

        }

        public void DeleteDataSet(string name)
        {
            var filter = Builders<IDataSet>.Filter
                .Eq("Name", name);
            Db.GetCollection<IDataSet>(CollectionName).DeleteOneAsync(filter).Wait();
        }

        public IEnumerable<IDataSet> GetDataSets()
        {
            var result = Db.GetCollection<IDataSet>(CollectionName).FindAsync(p => true);
            result.Wait();
            return result.Result.ToList();
        }

        public void DeleteAllDataSets()
        {
            Db.GetCollection<IDataSet>(CollectionName).DeleteManyAsync(prop => true).Wait();
        } 

        private IMongoDatabase Db { set; get; }
        private string CollectionName { set; get; }
    }
}
