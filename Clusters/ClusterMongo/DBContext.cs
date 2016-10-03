using System.Collections.Generic;
using ClusterDomain;

namespace ClusterMongo
{
    public interface DBContext
    {
        IEnumerable<DataSetInterface> DataSetAsEnumerable();
        void SaveDataSet(DataSetInterface dataSet);
        void DeleteDataSet(string name);
    }
}
