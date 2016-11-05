using System;
using System.Collections.Generic;
using System.Linq;
using ClusterDomain;
using FluentAssertions;
using Xunit;

namespace ClusterDomainTest
{
    public class DbScanClusterizerIntegration
    {
        [Fact]
        public void ClusterizerCorrectlyWorksWithEuclideanDistance()
        {
            // Arrange
            var metric = new PNorm(2);
            var clasterizer = new DbscanClasterizer(1, 3);

            var cluster1 = new HashSet<DataPoint>
            {
                new DataPoint(0,0),
                new DataPoint(0.5, 0),
                new DataPoint(0.6, 0.1),
                new DataPoint(1, 0),
                new DataPoint(1, 0.5),
                new DataPoint(-0.2, 0.1),
            };

            var cluster2 = new HashSet<DataPoint>
            {
                new DataPoint(10, 0.2),
                new DataPoint(10.5, 0),
                new DataPoint(10.6, 0.1),
                new DataPoint(11, 0),
                new DataPoint(11, 0.5),
            };

            var cluster3 = new HashSet<DataPoint>
            {
                new DataPoint(21, 0),
                new DataPoint(20, 0),
                new DataPoint(22, 0),

                // Эти две точки находятся близко к cluster3, 
                // но Евклидово расстояние < 1
                new DataPoint(19.5, 0.86),
                new DataPoint(19.0, 0.1)
            };

            var noise = new HashSet<DataPoint>
            {
                new DataPoint(100, 5),
                new DataPoint(100.1, 5),
                new DataPoint(-200, 1000),
            };

            var dataSet = new DataSet(cluster1.Union(cluster2).Union(cluster3).Union(noise));

            // Act
            clasterizer.Clusterize(metric, dataSet);

            var clusters = clasterizer.GetClusters();
            var n = clasterizer.GetNoise();

            // Assert
            clusters.Should().HaveCount(3)
                .And.Contain(x => cluster1.SetEquals(x.Points))
                .And.Contain(x => cluster2.SetEquals(x.Points))
                .And.Contain(x => cluster3.SetEquals(x.Points));

            n.Points.ShouldBeEquivalentTo(noise);
        }

        [Fact]
        public void ClusterizerCorrectlyWorksWithManhattanDistance()
        {
            // Arrange
            var metric = new PNorm(1);
            var clasterizer = new DbscanClasterizer(1, 3);

            var cluster1 = new HashSet<DataPoint>
            {
                new DataPoint(0,0),
                new DataPoint(0.5, 0),
                new DataPoint(0.6, 0.1),
                new DataPoint(1, 0),
                new DataPoint(1, 0.5),
                new DataPoint(-0.2, 0.1),
            };

            var cluster2 = new HashSet<DataPoint>
            {
                new DataPoint(10, 0.2),
                new DataPoint(10.5, 0),
                new DataPoint(10.6, 0.1),
                new DataPoint(11, 0),
                new DataPoint(11, 0.5),
            };

            var cluster3 = new HashSet<DataPoint>
            {
                new DataPoint(21, 0),
                new DataPoint(20, 0),
                new DataPoint(22, 0),

            };


            var noise = new HashSet<DataPoint>
            {
                new DataPoint(100, 5),
                new DataPoint(100.1, 5),
                new DataPoint(-200, 1000),

                // Эти две точки находятся близко к cluster3, 
                // но Евклидово расстояние > 1
                new DataPoint(19.5, 0.86),
                new DataPoint(19.0, 0.1)
            };

            var dataSet = new DataSet(cluster1.Union(cluster2).Union(cluster3).Union(noise));

            // Act
            clasterizer.Clusterize(metric, dataSet);

            var clusters = clasterizer.GetClusters();
            var n = clasterizer.GetNoise();

            // Assert
            clusters.Should().HaveCount(3)
                .And.Contain(x => cluster1.SetEquals(x.Points))
                .And.Contain(x => cluster2.SetEquals(x.Points))
                .And.Contain(x => cluster3.SetEquals(x.Points));

            n.Points.ShouldBeEquivalentTo(noise);
        }

        [Fact]
        public void ClusterizerCorrectlyWorksWithSupremumDistance()
        {
            // Arrange
            var metric = new SupNorm();
            var clasterizer = new DbscanClasterizer(1, 3);

            var cluster1 = new HashSet<DataPoint>
            {
                new DataPoint(0,0),
                new DataPoint(0.9, 0.2),
                new DataPoint(1, 0),
                new DataPoint(1, 0.5),
                new DataPoint(-0.8, 0.9),
            };

            var cluster2 = new HashSet<DataPoint>
            {
                new DataPoint(10, 0.2),
                new DataPoint(10.5, 0),
                new DataPoint(10.6, 0.1),
                new DataPoint(11, 0),
                new DataPoint(11, 0.5),
            };

            var noise = new HashSet<DataPoint>
            {
                new DataPoint(100, 5),
                new DataPoint(100.1, 5),
                new DataPoint(-200, 1000),
            };

            var dataSet = new DataSet(cluster1.Union(cluster2).Union(noise));

            // Act
            clasterizer.Clusterize(metric, dataSet);

            var clusters = clasterizer.GetClusters();
            var n = clasterizer.GetNoise();

            // Assert
            clusters.Should().HaveCount(2)
                .And.Contain(x => cluster1.SetEquals(x.Points))
                .And.Contain(x => cluster2.SetEquals(x.Points));

            n.Points.ShouldBeEquivalentTo(noise);
        }
    }
}