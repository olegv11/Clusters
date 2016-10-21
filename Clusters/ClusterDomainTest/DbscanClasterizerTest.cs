using System;
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
        public void DbscanClasterizerShouldDetectNoise()
        {
            // Arrange
            var noisePoint = A.Dummy<DataPoint>();

            var list = new HashSet<DataPoint>
            {
                A.Dummy<DataPoint>(),
                A.Dummy<DataPoint>(),
                A.Dummy<DataPoint>(),
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
                .NotContain(x => x.Contains(noisePoint), "ни в одном кластере не должно быть шума")
                .And.HaveCount(1);
            clusters[0].Points.Should().HaveCount(list.Count - 1);
            noise.Points.Should().HaveCount(1, "должна быть одна точка шума")
                .And.Contain(noisePoint, "noisePoint должен попасть в шум");
        }
        
        [Fact]
        public void DbscanClusterizerShouldFindClusters()
        {
            // Arrange
            var cluster1 = new HashSet<DataPoint>
            {
                A.Dummy<DataPoint>(),
                A.Dummy<DataPoint>(),
                A.Dummy<DataPoint>(),
                A.Dummy<DataPoint>(),
            };

            var cluster2 = new HashSet<DataPoint>
            {
                A.Dummy<DataPoint>(),
                A.Dummy<DataPoint>(),
                A.Dummy<DataPoint>(),
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
        void DbscanClusterizerShouldProcessEmptyData()
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

        [Theory]
        [InlineData(-2.3)]
        [InlineData(double.NaN)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.NegativeInfinity)]
        public void DbScanClasterizerShouldThrowOnInvalidEpsilon(double eps)
        {
            // Arrange
            Action createClasterizer = () => new DbscanClasterizer(eps, 1);

            // Assert
            createClasterizer.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(-15)]
        [InlineData(-1)]
        [InlineData(0)]
        public void DbScanClasterizerShouldThrowOnInvalidMinPoints(int minPoints)
        {
            // Arrange
            Action createClasterizer = () => new DbscanClasterizer(1, minPoints);

            // Assert
            createClasterizer.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void DbScanClasterizerShouldNotThrowOnCorrectInput()
        {
            // Arrange
            Action createClasterizer = () => new DbscanClasterizer(2, 5);

            // Assert
            createClasterizer.ShouldNotThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void DbScanClasterizerShouldThrowOnNullMetric()
        {
            // Arrange
            var dataSet = A.Fake<DataSet>();
            DbscanClasterizer clasterizer = new DbscanClasterizer(1,1);

            Action wrongMetric = () => clasterizer.Clusterize(null, dataSet);

            // Assert
            wrongMetric.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void DbScanClasterizerShouldThrowOnNullDataSet()
        {
            // Arrange
            var metric = A.Fake<Metric>();
            DbscanClasterizer clasterizer = new DbscanClasterizer(1, 1);

            Action wrongSet = () => clasterizer.Clusterize(metric, null);

            // Assert
            wrongSet.ShouldThrow<ArgumentNullException>();
        }


    }
}
