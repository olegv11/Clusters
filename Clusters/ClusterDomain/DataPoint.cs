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
            if (values.Count() < 2)
            {
                throw new ArgumentException("Should have at least 2 coordinates");
            }
            Values = new List<double>(values);
        }

        public DataPoint(params double[] values)
        {
            if (values.Count() < 2)
            {
                throw new ArgumentException("Should have at least 2 coordinates");
            }
            Values = new List<double>(values);
        }

        public int Dimension => Values.Count;

        public List<double> Values { get; }

        public Double X
        {
            get { return Values[0]; }
        }

        public Double Y
        {
            get { return Values[1]; }
        }
    }
}
