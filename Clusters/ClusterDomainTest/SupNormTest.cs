using System;
using System.Collections.Generic;
using System.Linq;
using ClusterDomain;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace ClusterDomainTest
{
    public class SupNormTest
    {
        public static IEnumerable<DataPoint[]> GetPointsWithNull => new[]
        {
            new DataPoint[] {null, null},
            new DataPoint[] {A.Fake<DataPoint>(), null},
            new DataPoint[] {null, A.Fake<DataPoint>()},
        };

        [Theory, MemberData(nameof(GetPointsWithNull))]
        public void SupNormShouldThrowIfAnyParameterIsNull(DataPoint x, DataPoint y)
        {
            // Arrange
            var measure = new SupNorm();
            Action calculateDistance = () => measure.DistanceBetween(x, y);

            // Assert
            calculateDistance.ShouldThrow<ArgumentNullException>();
        }

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
            var point2 = new DataPoint(new double[] {2, 6});
            var manuallyCalculatedDistance =
                point1.Values.Zip(point2.Values, (a, b) => Math.Abs(a - b)).Max();

            // Act
            var distance = measure.DistanceBetween(point1, point2);

            // Assert
            distance.Should().Be(manuallyCalculatedDistance, "супремум норма - максимум координат");
        }
    }
}