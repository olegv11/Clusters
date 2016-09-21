using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using ClusterDomain;

namespace ClusterMongo
{
    public class MongoDBRepository : DataSetRepository
    {
        private MongoDBContext context;

        public MongoDBRepository(MongoDBContext initContext)
        {
            context = initContext;
        }

        public IEnumerable<DataSet> GetAllDataSets()
        {
            return context.DataSetAsEnumerable();
        }

        public DataSet GetDataSetByName(string name)
        {
            var result = context.DataSetAsEnumerable().Where(x => x.Name == name);

            if (result.Count() == 0)
            {
                throw new MongoDBException("Не найдено подходящего набора данных");
            }

            if (result.Count() > 1)
            {
                throw new MongoDBException("Найден повтор в базе данных");
            }

            return result.First();
            
        }

        public void SaveDataSet(DataSet item)
        {
            context.DataSet.InsertOne(item);
        }

        public void DeleteDataSet(string name)
        {
            if (!context.DataSet.DeleteOne(x => x.Name == name).IsAcknowledged)
            {
                throw new MongoDBException();
            }
        }
    }
}
