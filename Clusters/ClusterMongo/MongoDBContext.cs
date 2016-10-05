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
            DataSet = database.GetCollection<IDataSet>("DataSet");
        }

        public void SaveDataSet(IDataSet dataSet)
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

        public IEnumerable<IDataSet> GetDataSets()
        {
            return DataSet.AsQueryable().ToEnumerable();
        } 

        private IMongoCollection<IDataSet> DataSet { set; get; }
    }
}
