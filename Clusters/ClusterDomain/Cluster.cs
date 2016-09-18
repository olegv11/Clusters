using System.Collections.Generic;

namespace ClusterDomain
{
    public class Cluster
    {
        public Cluster()
        {
            Points = new HashSet<DataPoint>();
        }

        public Cluster(IEnumerable<DataPoint> points)
        {
            Points = new HashSet<DataPoint>(points);
        }

        public void AddPoint(DataPoint point)
        {
            Points.Add(point);
        }

        public int Count => Points.Count;

        public bool Empty => Count == 0;

        public HashSet<DataPoint> Points { get; }

    }
}