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
            var filter = Builders<IDataSet>.Filter
                              .Eq("Name", dataSet.Name);

            long count = Db.GetCollection<IDataSet>(CollectionName).Count(filter);
            
            if (count == 0)
            {
                Db.GetCollection<IDataSet>(CollectionName).InsertOneAsync(dataSet);
            }
            else if(count == 1)
            {
                Db.GetCollection<IDataSet>(CollectionName).DeleteOne(filter);
                Db.GetCollection<IDataSet>(CollectionName).InsertOneAsync(dataSet);
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
            var count = Db.GetCollection<IDataSet>(CollectionName).Find(filter).Count();
            if (count == 0)
            {
                throw new MongoDBException("Не найдено подходящего набора данных");
            }
            else if (count == 1)
            {
                Db.GetCollection<IDataSet>(CollectionName).DeleteOneAsync(filter);
            }
            else
            {
                throw new MongoDBException("Найден повтор в базе данных");
            }
        }

        public IEnumerable<IDataSet> GetDataSets()
        {
            return Db.GetCollection<IDataSet>(CollectionName).FindAsync(p => true).Result.ToList();
        }

        public void DeleteAllDataSets()
        {
            Db.GetCollection<IDataSet>(CollectionName).DeleteManyAsync(prop => true);
        } 

        private IMongoDatabase Db { set; get; }
        private string CollectionName { set; get; }
    }
}
