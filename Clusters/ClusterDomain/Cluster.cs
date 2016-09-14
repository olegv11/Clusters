using System.Collections.Generic;

namespace ClusterDomain
{
    public class Cluster
    {
        public Cluster()
        {
            DataPoints = new List<DataPoint>();
        }

        public Cluster(IEnumerable<DataPoint> points)
        {
            DataPoints = new List<DataPoint>(points);
        }

        public void AddPoint(DataPoint point)
        {
            DataPoints.Add(point);
        }

        public void AddRange(IEnumerable<DataPoint> points)
        {
            DataPoints.AddRange(points);
        }

        public int Count => DataPoints.Count;

        public bool Empty => DataPoints.Count == 0;

        public List<DataPoint> DataPoints { get; }
    }
}