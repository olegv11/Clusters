using System.Collections.Generic;
using MongoDB.Driver;
using ClusterDomain;
using MongoDB.Driver.Linq;

namespace ClusterMongo
{
    public class MongoDBContext
    {
        public MongoDBContext() { }

        public MongoDBContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            DataSet = database.GetCollection<DataSet>("DataSet");
        }

        public virtual IEnumerable<DataSet> DataSetAsEnumerable()
        {
            return DataSet.AsQueryable().ToEnumerable();
        }

        public virtual IMongoCollection<DataSet> DataSet { set; get; }
    }
}
