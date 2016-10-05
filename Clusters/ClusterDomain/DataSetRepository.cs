using System.Collections.Generic;

namespace ClusterDomain
{
    public interface DataSetRepository
    {
        IEnumerable<IDataSet> GetAllDataSets();
        IDataSet GetDataSetByName(string name);
        void SaveDataSet(IDataSet dataSet);
        void DeleteDataSet(string name);
    }
}