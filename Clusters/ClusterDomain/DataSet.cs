using System;
using System.Collections.Generic;

namespace ClusterDomain
{
    public class DataSet : IDataSet
    {
        public Guid Id { get; private set; }
        public DataSet()
        {
            Id = Guid.NewGuid();
            Data = new HashSet<DataPoint>();
        }

        public DataSet(IEnumerable<DataPoint> points)
        {
            Id = Guid.NewGuid();
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

        private string customName = null;

        public string Name
        {
            get
            {
                if (customName != null)
                {
                    return customName;
                }
                else
                {
                    return Id.ToString();
                }
            }
            set
            {
                if (customName != value)
                {
                    customName = value;
                }
            }
        }
    }
}