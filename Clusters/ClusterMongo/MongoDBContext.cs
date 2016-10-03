using System.Collections.Generic;
using MongoDB.Driver;
using ClusterDomain;
using MongoDB.Driver.Linq;

namespace ClusterMongo
{
    public class MongoDBContext : DBContext
    {
        public MongoDBContext() { }

        public MongoDBContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            DataSet = database.GetCollection<DataSet>("DataSet");
        }

        public IEnumerable<DataSet> DataSetAsEnumerable()
        {
            return DataSet.AsQueryable().ToEnumerable();
        }

        public void SaveDataSet(DataSet dataSet)
        {
            DataSet.InsertOne(dataSet);
        }

        public void DeleteDataSet(string name)
        {
            if (!DataSet.DeleteOne(x => x.Name == name).IsAcknowledged)
            {
                throw new MongoDBException("Ошибка при удалении");
            }
        }

        public IMongoCollection<DataSet> DataSet { set; get; }
    }
}
