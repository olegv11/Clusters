using System;
using System.Linq;
using ClusterDomain;
using FluentAssertions;
using Xunit;

namespace ClusterDomainTest
{
    public class SupNormTest
    {
        [Fact]
        public void SupNormShouldAllowOnlyDataSetsWithSameDimension()
        {
            // Arrange
            var measure = new SupNorm();
            var point1 = new DataPoint(new double[] { 1, 3 });
            var point2 = new DataPoint(new double[] { 6, -10, 12, 22 });
            var point3 = new DataPoint(new double[] { 5, 0 });

            Action doMeasureWrong = () => measure.DistanceBetween(point1, point2);
            Action doMeasureRight = () => measure.DistanceBetween(point1, point3);

            // Assert
            doMeasureWrong.ShouldThrow<ArgumentException>("точки имеют различную размерность");
            doMeasureRight.ShouldNotThrow<ArgumentException>("точки имеют одинаковую размерность");
        }

        [Fact]
        public void SupNormIsMaxOfCoordinates()
        {
            // Arrange
            var measure = new SupNorm();
            var point1 = new DataPoint(new double[] {5, 4});
            var point2 = new DataPoint(new double[] {6, 6});
            var manuallyCalculatedDistance =
                point1.Values.Zip(point2.Values, (a, b) => Math.Abs(a - b)).Max();

            // Act
            var distance = measure.DistanceBetween(point1, point2);

            // Assert
            distance.Should().Be(manuallyCalculatedDistance, "супремум норма - максимум координат");
        }
    }
}