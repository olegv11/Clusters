using System.Collections.Generic;

namespace ClusterDomain
{
    public interface DataSetRepository
    {
        IEnumerable<DataSet> GetAllDataSets();
        DataSet GetDataSetByName(string name);
        void SaveDataSet(DataSet dataSet);
        void DeleteDataSet(string name);
    }
}