using System.Collections;
using System.Collections.Generic;

namespace ClusterDomain
{
    public interface Clusterizer
    {
        void Clusterize(IList<DataPoint> dataSet);
        List<Cluster> GetClusters();
        Cluster GetNoise();
    }
}