using System;
using System.Linq;

namespace ClusterDomain
{
    public class SupNorm : Metric
    {
        public double DistanceBetween(DataPoint x, DataPoint y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));

            if (x.Dimension != y.Dimension)
            {
                throw new ArgumentException("оба параметра должны совпадать по размерности");
            }

            return x.Values.Zip(y.Values, (a, b) => Math.Abs(a - b)).Max();
        }
    }
}