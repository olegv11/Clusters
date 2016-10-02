using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ClusterDomain
{
    public class DataPoint
    {
        public DataPoint(IEnumerable<double> values)
        {
            Values = new List<double>(values);
        }

        public DataPoint(params double[] values)
        {
            Values = new List<double>(values);
        }

        public int Dimension => Values.Count;

        public List<double> Values { get; }
    }
}
