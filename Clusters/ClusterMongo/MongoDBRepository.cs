﻿using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using ClusterDomain;

namespace ClusterMongo
{
    public class DBRepository : DataSetRepository
    {
        private DBContext context;

        public DBRepository(DBContext initContext)
        {
            context = initContext;
        }

        public IEnumerable<DataSetInterface> GetAllDataSets()
        {
            return context.DataSetAsEnumerable();
        }

        public DataSetInterface GetDataSetByName(string name)
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

        public void SaveDataSet(DataSetInterface item)
        {
            context.SaveDataSet(item);
        }

        public void DeleteDataSet(string name)
        {
            context.DeleteDataSet(name);
        }
    }
}
