using System.Collections.Generic;

namespace ClusterDomain
{
    public interface DataSetRepository
    {
        IEnumerable<DataSetInterface> GetAllDataSets();
        DataSetInterface GetDataSetByName(string name);
        void SaveDataSet(DataSetInterface dataSet);
        void DeleteDataSet(string name);
    }
}