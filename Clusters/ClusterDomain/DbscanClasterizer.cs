using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusterDomain
{
    public class DbscanClasterizer : Clusterizer
    {
        public DbscanClasterizer(double epsilon, int minPoints)
        {
            this.epsilon = epsilon;
            this.minPoints = minPoints;
        }

        public void Clusterize(DataSet dataSet)
        {
            clusters = new List<Cluster>();
            visited = new HashSet<DataPoint>();
            noise = new DataSet();

            foreach (var point in dataSet.Data)
            {
                if (visited.Contains(point))
                {
                    continue;
                }

                visited.Add(point);

                var neighbours = RegionQuery(point, dataSet);
                if (neighbours.Count < minPoints)
                {
                    noise.AddPoint(point);
                }
                else
                {
                    var currentCluster = new Cluster();                   
                    ExpandCluster(point, neighbours, currentCluster, dataSet);
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


        private void ExpandCluster(DataPoint center, List<DataPoint> neighbours, Cluster cluster, DataSet dataSet)
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

                var pointNeighbours = RegionQuery(point, dataSet);

                if (pointNeighbours.Count >= minPoints)
                {
                    neighbours = neighbours.Union(pointNeighbours).ToList();
                }
            }
        }

        private List<DataPoint> RegionQuery(DataPoint center, DataSet dataSet)
        {
            return dataSet.Data.Where(x => center.DistanceTo(x) <= epsilon).ToList();
        }

        private List<Cluster> clusters;
        private HashSet<DataPoint> visited;
        private DataSet noise;
        private double epsilon;
        private double minPoints;
    }
}