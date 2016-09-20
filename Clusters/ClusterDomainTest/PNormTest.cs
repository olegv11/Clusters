using System;
using System.Linq;
using ClusterDomain;
using FluentAssertions;
using Xunit;

namespace ClusterDomainTest
{
    public class PNormTest
    {
        [Fact]
        public void PNormShouldNotAllowIllegalP()
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
        public void ManhattanDistanceIsSumOfElements()
        {
            // Arrange
            var manhattan = new PNorm(1);
            var point1 = new DataPoint(new double[] { 1, 2, 3 });
            var point2 = new DataPoint(new double[] { 5, -10, 17 });
            var manuallyCalculatedDistance =
                point1.Values.Zip(point2.Values, (a, b) => Math.Abs(a - b)).Sum();
            // Act
            double distance = manhattan.DistanceBetween(point1, point2);

            // Assert
            distance.Should().Be(manuallyCalculatedDistance, 
                "манхэттенское растояние - сумма разностей соответсвующих координат");
        }

        [Fact]
        public void EuclideanNormIsSquarRootOfSumOfSquaredElements()
        {
            // Arrange
            var euclid = new PNorm(2);
            var point1 = new DataPoint(new double[] {1, 3});
            var point2 = new DataPoint(new double[] {-10, 5});
            var manuallyCalculatedDistance =
                Math.Sqrt(point1.Values.Zip(point2.Values, (a, b) => Math.Pow(a - b, 2)).Sum());

            // Act
            double distance = euclid.DistanceBetween(point1, point2);

            // Assert
            distance.Should().Be(manuallyCalculatedDistance,
                "евклидова норма - корень из суммы квадратор разностей соответствующих координат");
        }

        [Fact]
        public void PNormShouldAllowOnlyDataSetsWithSameDimension()
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