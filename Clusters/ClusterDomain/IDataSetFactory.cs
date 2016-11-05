using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterDomain
{
    public interface IDataSetFactory
    {
        IDataSet EmptyDataSet();
        IDataSet DataSetFromIEnumarableOfPoints(IEnumerable<DataPoint> points);
    }
}
