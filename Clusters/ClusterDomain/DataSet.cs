using System;
using System.Collections.Generic;

namespace ClusterDomain
{
    public class DataSet : IDataSet
    {
        public int Id { get; private set; }

        public DataSet()
        {
            Data = new HashSet<DataPoint>();
        }

        public DataSet(IEnumerable<DataPoint> points)
        {
            Data = new HashSet<DataPoint>(points);
        }

        public void AddPoint(DataPoint point)
        {
            Data.Add(point);
        }

        public void AddPoints(IEnumerable<DataPoint> points)
        {
            Data.UnionWith(points);
        }

        public bool HasPoint(DataPoint point)
        {
            return Data.Contains(point);
        }

        public HashSet<DataPoint> Data { get; }

        public DateTime CreationTime { get; set; }

        public string Name { get; set; }
    }
}