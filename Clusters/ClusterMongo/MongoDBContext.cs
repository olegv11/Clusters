using System.Collections.Generic;
using MongoDB.Driver;
using ClusterDomain;
using MongoDB.Driver.Linq;

namespace ClusterMongo
{
    public class MongoDBContext : DBContext
    {
        public MongoDBContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            DataSet = database.GetCollection<DataSetInterface>("DataSet");
        }

        public IEnumerable<DataSetInterface> DataSetAsEnumerable()
        {
            return DataSet.AsQueryable().ToEnumerable();
        }

        public void SaveDataSet(DataSetInterface dataSet)
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

        public IMongoCollection<DataSetInterface> DataSet { set; get; }
    }
}
