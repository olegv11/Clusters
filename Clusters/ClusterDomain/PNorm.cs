using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterDomain
{
    public class PNorm : Metric
    {
        public PNorm(double p)
        {
            if (p < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(p));
            }
            this.p = p;
        }

        public double DistanceBetween(DataPoint x, DataPoint y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));

            if (x.Dimension != y.Dimension)
            {
                throw new ArgumentException("оба параметра должны совпадать по размерности");
            }

            return Math.Pow(x.Values.Zip(y.Values, (a, b) => Math.Pow(Math.Abs(a-b), p)).Sum(), 1/p);
        }

        private double p;
    }
}
