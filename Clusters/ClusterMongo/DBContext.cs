using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
