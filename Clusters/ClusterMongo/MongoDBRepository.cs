﻿using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using ClusterDomain;
using System;
using Ninject;

namespace ClusterMongo
{
    public class DBRepository : DataSetRepository
    {
        private DBContext context;
        
        public DBRepository(DBContext initContext)
        {
            if(initContext == null)
            {
                throw new ArgumentException("Некорректный параметр конструктора");
            }
            context = initContext;
        }

        public IEnumerable<IDataSet> GetAllDataSets()
        {
            return context.GetDataSets();
        }

        public IDataSet GetDataSetByName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (context.GetDataSets().Count() == 0)
            {
                throw new MongoDBException("Не найдено подходящего набора данных");
            }

            var result = context.GetDataSets().Where(x => x.Name == name);

            if ((result == null) || (result.Count() == 0))
            {
                throw new MongoDBException("Не найдено подходящего набора данных");
            }

            if (result.Count() > 1)
            {
                throw new MongoDBException("Найден повтор в базе данных");
            }

            return result.First();
            
        }

        public void SaveDataSet(IDataSet item)
        {
            if(item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            
            context.SaveDataSet(item);
        }

        public void DeleteDataSet(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            context.DeleteDataSet(name);
        }

        public void DeleteAllDataSets()
        {
            context.DeleteAllDataSets();
        }
    }
}
