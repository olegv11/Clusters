﻿using System;
using ClusterDomain;
using FluentAssertions;
using Xunit;

namespace ClusterDomainTest
{
    public class PNormTest
    {
        [Fact]
        public void TestPNormShouldNotAllowIllegalP()
        {
            // Arrange
            Action<double> createNorm = (double p) => new PNorm(p);


            // Assert
            createNorm.Invoking(x => x.Invoke(0.5))
                .ShouldThrow<ArgumentOutOfRangeException>("0.5 меньше 1");
            createNorm.Invoking(x => x.Invoke(0))
                .ShouldThrow<ArgumentOutOfRangeException>("0 меньше 1");

            createNorm.Invoking(x => x.Invoke(1))
                .ShouldNotThrow<ArgumentOutOfRangeException>("1 больше или равно 1");
            createNorm.Invoking(x => x.Invoke(500))
                .ShouldNotThrow<ArgumentOutOfRangeException>("500 больше или равно 1");
        }

        [Fact]
        public void TestManhattanDistanceIsSumOfElements()
        {
            // Arrange
            var manhattan = new PNorm(1);
            var point1 = new DataPoint(new double[] { 1, 2, 3 });
            var point2 = new DataPoint(new double[] { 5, -10, 17 });

            // Act
            double distance = manhattan.DistanceBetween(point1, point2);

            // Assert
            distance.Should().Be((5 - 1) + (-(-10 - 2)) + (17 - 3), 
                "манхэттенское растояние - сумма разностей соответсвующих координат");
        }

        [Fact]
        public void TestEuclideanNormIsSquarRootOfSumOfSquaredElements()
        {
            // Arrange
            var euclid = new PNorm(2);
            var point1 = new DataPoint(new double[] {1, 3});
            var point2 = new DataPoint(new double[] {-10, 5});

            // Act
            double distance = euclid.DistanceBetween(point1, point2);

            // Assert
            distance.Should().Be(Math.Sqrt(Math.Pow(1 + 10, 2) + Math.Pow(3 - 5, 2)),
                "евклидова норма - корень из суммы квадратор разностей соответствующих координат");
        }

        [Fact]
        public void TestPNormShouldAllowOnlyDataSetsWithSameDimension()
        {
            // Arrange
            var measure = new PNorm(10);
            var point1 = new DataPoint(new double[] { 1, 3 });
            var point2 = new DataPoint(new double[] { 6, -10, 12, 22 });
            var point3 = new DataPoint(new double[] { 5, 0 });

            Action doMeasureWrong = () => measure.DistanceBetween(point1, point2);
            Action doMeasureRight = () => measure.DistanceBetween(point1, point3);

            // Assert
            doMeasureWrong.ShouldThrow<ArgumentException>("точки имеют различную размерность");
            doMeasureRight.ShouldNotThrow<ArgumentException>("точки имеют одинаковую размерность");
        }
    }
}