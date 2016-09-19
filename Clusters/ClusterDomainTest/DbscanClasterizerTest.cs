using System.Collections.Generic;
using System.Linq;
using Xunit;
using ClusterDomain;
using FakeItEasy;
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
            var noisePoint = A.Fake<DataPoint>();

            var list = new HashSet<DataPoint>
            {
                new DataPoint(new double[] {0, 0}),
                new DataPoint(new double[] {0.5, 0}),
                new DataPoint(new double[] {1, 0.5}),
                noisePoint,
            };
            var dataset = new DataSet(list);
            DbscanClasterizer clasterizer = new DbscanClasterizer(2,3);

            var metric = A.Fake<Metric>();
            A.CallTo(() => metric.DistanceBetween(A<DataPoint>._, A<DataPoint>._)).Returns(0.5);
            A.CallTo(() => metric.DistanceBetween(A<DataPoint>._, noisePoint)).Returns(1000);
            A.CallTo(() => metric.DistanceBetween(noisePoint, A<DataPoint>._)).Returns(1000);

            // Act
            clasterizer.Clusterize(metric, dataset);

            var clusters = clasterizer.GetClusters();
            var noise = clasterizer.GetNoise();

            // Assert
            clusters.Select(x => x.Points).Should()
                .NotContain(x => x.Contains(noisePoint), "ни в одном кластере не должно быть шума");
            // Шум находится корректно
            noise.Points.Should().HaveCount(1, "должна быть одна точка шума")
                .And.Contain(noisePoint, "noisePoint должен попасть в шум");
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

            var metric = A.Fake<Metric>();
            A.CallTo(() => metric.DistanceBetween(A<DataPoint>._, A<DataPoint>._))
                .Returns(1000);
            A.CallTo(() => metric.DistanceBetween(A<DataPoint>._, A<DataPoint>._))
                .WhenArgumentsMatch(
                    args => cluster1.Contains(args.Get<DataPoint>("x")) && cluster1.Contains(args.Get<DataPoint>("y")))
                .Returns(0.5);
            A.CallTo(() => metric.DistanceBetween(A<DataPoint>._, A<DataPoint>._))
                .WhenArgumentsMatch(
                    args => cluster2.Contains(args.Get<DataPoint>("x")) && cluster2.Contains(args.Get<DataPoint>("y")))
                .Returns(0.6);

            // Act
            clasterizer.Clusterize(metric, dataset);

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
            var metric = A.Fake<Metric>();

            // Act
            clasterizer.Clusterize(metric, empty);

            var clusters = clasterizer.GetClusters();
            var noise = clasterizer.GetNoise();

            // Assert
            clusters.Should().BeEmpty("не должно быть найдено кластеров");
            noise.Points.Should().BeEmpty("не должно быть найдено шума");
        }

    }
}
