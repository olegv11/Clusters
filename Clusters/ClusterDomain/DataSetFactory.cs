using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterDomain
{
    public class DataSetFactory : IDataSetFactory
    {
        IDataSet IDataSetFactory.DataSetFromIEnumarableOfPoints(IEnumerable<DataPoint> points)
        {
            return new DataSet(points);
        }

        IDataSet IDataSetFactory.EmptyDataSet()
        {
            return new DataSet();
        }
    }
}
