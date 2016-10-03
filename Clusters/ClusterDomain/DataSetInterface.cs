using System;
using System.Collections.Generic;

namespace ClusterDomain
{
    public interface DataSetInterface
    {
        DateTime CreationTime { get; set; }
        HashSet<DataPoint> Data { get; }
        string Name { get; set; }

        void AddPoint(DataPoint point);
        void AddPoints(IEnumerable<DataPoint> points);
        bool HasPoint(DataPoint point);
    }
}