using System.Collections;
using System.Collections.Generic;

namespace ClusterDomain
{
    public interface Clusterizer
    {
        void Clusterize(Metric metric, IDataSet dataSet);
        List<Cluster> GetClusters();
        Cluster GetNoise();
    }
}