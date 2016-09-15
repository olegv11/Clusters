using System.Collections.Generic;

namespace ClusterDomain
{
    public class Cluster
    {
        public Cluster()
        {
            DataPoints = new HashSet<DataPoint>();
        }

        public Cluster(IEnumerable<DataPoint> points)
        {
            DataPoints = new HashSet<DataPoint>(points);
        }

        public void AddPoint(DataPoint point)
        {
            DataPoints.Add(point);
        }

        public int Count => DataPoints.Count;

        public bool Empty => DataPoints.Count == 0;

        public HashSet<DataPoint> DataPoints { get; }
    }
}