using System.Collections.Generic;

namespace ClusterDomain
{
    public class Cluster
    {
        public Cluster()
        {
            dataset = new DataSet();
        }

        public Cluster(DataSet points)
        {
            dataset = new DataSet(points);
        }

        public void AddPoint(DataPoint point)
        {
            dataset.AddPoint(point);
        }

        public int Count => Points.Count;

        public bool Empty => Count == 0;

        public DataSet dataset;

        public HashSet<DataPoint> Points => dataset.Data;
    }
}