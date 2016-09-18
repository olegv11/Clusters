using System.Collections.Generic;
using System.Linq;
using Xunit;
using ClusterDomain;
using FluentAssertions;
using FluentAssertions.Common;

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

            var list = new List<DataPoint>()
            {
                new DataPoint(new double[] {0, 0}),
                new DataPoint(new double[] {0.5, 0}),
                new DataPoint(new double[] {1, 0.5}),
                noise1,
                noise2
            };
            var dataset = new DataSet(list);

            DbscanClasterizer clasterizer = new DbscanClasterizer(2,3);

            // Act
            clasterizer.Clusterize(dataset);

            var clusters = clasterizer.GetClusters();
            var noise = clasterizer.GetNoise();

            // Assert
            clusters.Select(x => x.Points).Should()
                .NotContain(x => x.Contains(noise1), "в кластере не должно быть шума").And
                .NotContain(x => x.Contains(noise2), "в кластере не должно быть шума");

            // Шум находится корректно
            noise.Points.Should().HaveCount(2, "должно быть две точки шума")
                .And.Contain(noise1, "noise1 должен попасть в шум")
                .And.Contain(noise2, "noise2 должен попасть в шум");
        }

        [Fact]
        public void TestDbscanClusterizerShouldFindClusters()
        {
            // Arrange
            var cluster1 = new HashSet<DataPoint>
            {
                new DataPoint(new double[] {0, 0}),
                new DataPoint(new double[] {0.5, 0.5}),
                new DataPoint(new double[] {-1, 0}),
                new DataPoint(new double[] {2, 2})
            };

            var cluster2 = new HashSet<DataPoint>
            {
                new DataPoint(new double[] {10, 10}),
                new DataPoint(new double[] {9, 11}),
                new DataPoint(new double[] {12, 10})
            };

            DbscanClasterizer clasterizer = new DbscanClasterizer(5, 2);
            var dataset = new DataSet(cluster1.Union(cluster2));

            // Act
            clasterizer.Clusterize(dataset);

            var clusters = clasterizer.GetClusters();
            var noise = clasterizer.GetNoise();

            // Assert
            clusters.Should().HaveCount(2)
                .And.Contain(x => cluster1.SetEquals(x.Points), "первый кластер должен быть найден")
                .And.Contain(x => cluster2.SetEquals(x.Points), "второй кластер должен быть найден");

            noise.Points.Should().BeEmpty("шума не должно быть");
        }

        [Fact]
        void TestDbscanClusterizerShouldProcessEmptyData()
        {
            // Arrange
            var empty = new DataSet();

            DbscanClasterizer clasterizer = new DbscanClasterizer(1, 1);

            // Act
            clasterizer.Clusterize(empty);

            var clusters = clasterizer.GetClusters();
            var noise = clasterizer.GetNoise();

            // Assert
            clusters.Should().BeEmpty("не должно быть найдено кластеров");
            noise.Points.Should().BeEmpty("не должно быть найдено шума");
        }

    }
}
