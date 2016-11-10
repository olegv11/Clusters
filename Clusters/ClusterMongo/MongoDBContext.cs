using System.Collections.Generic;
using MongoDB.Driver;
using ClusterDomain;
using System.Linq;

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
            var set = new MongoSet(dataSet);
            var filter = Builders<MongoSet>.Filter
                              .Eq("Name", set.Name);

            long count = Db.GetCollection<MongoSet>(CollectionName).Count(filter);
            
            if (count == 0)
            {
                var t = Db.GetCollection<MongoSet>(CollectionName).InsertOneAsync(set);
                t.Wait();
            }
            else if(count == 1)
            {
                Db.GetCollection<MongoSet>(CollectionName).DeleteOne(filter);
                var t = Db.GetCollection<MongoSet>(CollectionName).InsertOneAsync(set);
                t.Wait();
            }
            else
            {
                throw new MongoDBException("Найден повтор в базе данных");
            }

        }

        public void DeleteDataSet(string name)
        {
            var filter = Builders<MongoSet>.Filter
                .Eq("Name", name);
            var count = Db.GetCollection<MongoSet>(CollectionName).Find(filter).Count();
            if (count == 0)
            {
                throw new MongoDBException("Не найдено подходящего набора данных");
            }
            else if (count == 1)
            {
                var t = Db.GetCollection<MongoSet>(CollectionName).DeleteOneAsync(filter);
                t.Wait();
            }
            else
            {
                throw new MongoDBException("Найден повтор в базе данных");
            }
        }

        public IEnumerable<IDataSet> GetDataSets()
        {
            return Db.GetCollection<MongoSet>(CollectionName).FindAsync(p => true).Result
                .ToList().Select(x => x.ToDataSet());
        }

        public void DeleteAllDataSets()
        {
            var t = Db.GetCollection<MongoSet>(CollectionName).DeleteManyAsync(prop => true);
            t.Wait();
        } 

        private IMongoDatabase Db { set; get; }
        private string CollectionName { set; get; }
    }
}
