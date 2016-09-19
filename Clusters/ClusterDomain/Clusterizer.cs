using System.Collections;
using System.Collections.Generic;

namespace ClusterDomain
{
    public interface Clusterizer
    {
        void Clusterize(Metric metric, DataSet dataSet);
        List<Cluster> GetClusters();
        Cluster GetNoise();
    }
}