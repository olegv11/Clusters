using System.Collections.Generic;
using System.Linq;
using Xunit;
using ClusterDomain;

namespace ClusterDomainTest
{
    public class DbscanClasterizerTest
    {
        [Fact]
        public void TestDbscanClasterizerShouldDetectNoise()
        {
            // Arrange
            var noise1 = new DataPoint(new double[] { 10, 10 });
            var noise2 = new DataPoint(new double[] { -5, 0 });

            var dataset = new List<DataPoint>()
            {
                new DataPoint(new double[] {0, 0}),
                new DataPoint(new double[] {0.5, 0}),
                new DataPoint(new double[] {1, 0.5}),
                noise1,
                noise2
            };

            DbscanClasterizer clasterizer = new DbscanClasterizer(2,3);

            // Act
            clasterizer.Clusterize(dataset);

            var clusters = clasterizer.GetClusters();
            var noise = clasterizer.GetNoise();

            // Assert
            // Шум не содержится ни в одном кластере
            Assert.False(clusters.Any(cluster => cluster.DataPoints.Contains(dataset[3])));
            Assert.False(clusters.Any(cluster => cluster.DataPoints.Contains(dataset[4])));

            // Шум находится корректно
            Assert.True(noise.Count == 2);
            Assert.True(noise.DataPoints.Contains(noise1));
            Assert.True(noise.DataPoints.Contains(noise2));
        }

        [Fact]
        public void TestDbscanClusterizerShouldFindClusters()
        {
            // Arrange
            var cluster1 = new List<DataPoint>(4)
            {
                new DataPoint(new double[] {0, 0}),
                new DataPoint(new double[] {0.5, 0.5}),
                new DataPoint(new double[] {-1, 0}),
                new DataPoint(new double[] {2, 2})
            };

            var cluster2 = new List<DataPoint>(3)
            {
                new DataPoint(new double[] {10, 10}),
                new DataPoint(new double[] {9, 11}),
                new DataPoint(new double[] {12, 10})
            };

            DbscanClasterizer clasterizer = new DbscanClasterizer(5, 2);
            var dataset = cluster1.Union(cluster2).ToList();

            // Act
            clasterizer.Clusterize(dataset);

            var clusters = clasterizer.GetClusters();
            var noise = clasterizer.GetNoise();

            //Assert
            //Нашли два кластера
            Assert.True(clusters.Count == 2);

            // В них нужные элементы
            Assert.True(clusters.Select(x => x.DataPoints).Any(x => AreListsEquivalent(x, cluster1)));
            Assert.True(clusters.Select(x => x.DataPoints).Any(x => AreListsEquivalent(x, cluster2)));

            // И нет шума
            Assert.True(noise.Empty);
        }


        public bool AreListsEquivalent<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            return (list1.Count() == list2.Count()) && !list1.Except(list2).Any();
        }
    }
}
