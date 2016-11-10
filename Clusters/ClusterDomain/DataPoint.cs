using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ClusterDomain
{
    public class DataPoint : IEquatable<DataPoint>
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

        public bool Equals(DataPoint other)
        {
            if (other == null)
                return false;
            return this.Dimension == other.Dimension && this.Values.SequenceEqual(other.Values);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as DataPoint);
        }

        public override int GetHashCode()
        {
            return Values.GetHashCode();
        }
    }
}
