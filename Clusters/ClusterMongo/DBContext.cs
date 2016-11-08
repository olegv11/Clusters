using MongoDB.Driver;
using System.Collections.Generic;
using ClusterDomain;

namespace ClusterMongo
{
    public interface DBContext
    {
        IEnumerable<IDataSet> GetDataSets();
        void SaveDataSet(IDataSet dataSet);
        void DeleteDataSet(string name);
        void DeleteAllDataSets();
    }
}
