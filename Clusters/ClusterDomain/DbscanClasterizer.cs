using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusterDomain
{
    public class DbscanClasterizer : Clusterizer
    {
        public DbscanClasterizer(double epsilon, int minPoints)
        {
            if (epsilon < 0 || double.IsInfinity(epsilon) || double.IsNaN(epsilon))
                throw new ArgumentOutOfRangeException(nameof(epsilon));
            if (minPoints <= 0)
                throw new ArgumentOutOfRangeException(nameof(minPoints));

            this.epsilon = epsilon;
            this.minPoints = minPoints;
        }

        public void Clusterize(Metric metric, IDataSet dataSet)
        {
            if (metric == null) throw new ArgumentNullException(nameof(metric));
            if (dataSet == null) throw new ArgumentNullException(nameof(dataSet));

            clusters = new List<Cluster>();
            visited = new HashSet<DataPoint>();
            noise = new HashSet<DataPoint>();

            currentMetric = metric;
            currentDataSet = dataSet;

            foreach (var point in dataSet.Data)
            {
                if (visited.Contains(point))
                {
                    continue;
                }

                visited.Add(point);

                var neighbours = RegionQuery(point);
                if (neighbours.Count < minPoints)
                {
                    noise.Add(point);
                }
                else
                {
                    var currentCluster = new Cluster();                   
                    ExpandCluster(point, neighbours, currentCluster);
                    clusters.Add(currentCluster);
                }
            }
        }

        public List<Cluster> GetClusters()
        {
            return clusters;
        }

        public Cluster GetNoise()
        {
            return new Cluster(noise);
        }

        private void ExpandCluster(DataPoint center, List<DataPoint> neighbours, Cluster cluster)
        {
            cluster.AddPoint(center);

            for (int i = 0; i < neighbours.Count; i++)
            {
                var point = neighbours[i];

                if (visited.Contains(point))
                {
                    continue;
                }

                cluster.AddPoint(point);
                visited.Add(point);

                var pointNeighbours = RegionQuery(point);

                if (pointNeighbours.Count >= minPoints)
                {
                    neighbours = neighbours.Union(pointNeighbours).ToList();
                }
            }
        }

        private List<DataPoint> RegionQuery(DataPoint center)
        {
            return currentDataSet.Data
                .Where(x => currentMetric.DistanceBetween(center, x) <= epsilon)
                .ToList();
        }

        private List<Cluster> clusters;
        private HashSet<DataPoint> visited;
        private HashSet<DataPoint> noise;

        private readonly double epsilon;
        private readonly double minPoints;

        private Metric currentMetric;
        private IDataSet currentDataSet;
    }
}