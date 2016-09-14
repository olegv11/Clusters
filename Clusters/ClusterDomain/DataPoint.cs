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

        public double DistanceTo(DataPoint other)
        {
            if (Dimension != other.Dimension)
            {
                throw new ArgumentException("Different data dimensions");
            }

            return Math.Sqrt(
                Values.Zip(other.Values, (x, y) => Math.Pow(x - y, 2)).Sum());
        }

        public int Dimension => Values.Count;

        private List<double> Values { get; }
    }
}
