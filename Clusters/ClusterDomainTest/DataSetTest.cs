using System.Collections;
using System.Collections.Generic;
using ClusterDomain;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace ClusterDomainTest
{
    public class DataSetTest
    {
        [Fact]
        public void DataSetShouldBeEmptyOnDefaulyConstructor()
        {
            // Arrange
            var dataSet = new DataSet();

            // Assert
            dataSet.Data.Should().BeEmpty("по умолчанию создаётся пустое множество данных");
        }

        [Fact]
        public void DataSetShouldContainAllElementsFromEnumerableSuppliedToConstructor()
        {
            // Arrange
            IEnumerable<DataPoint> enumerable = new List<DataPoint>
            {
                A.Fake<DataPoint>(),
                A.Fake<DataPoint>(),
                A.Fake<DataPoint>(),
            };

            // Act
            var dataSet = new DataSet(enumerable);

            // Assert
            dataSet.Data.Should().BeEquivalentTo(enumerable, 
                "при создании множества с помощью IEnumerable оно должно содержать все его элементы");
        }

        [Fact]
        public void DataSetShouldAddElement()
        {
            // Arrange
            var point = A.Fake<DataPoint>();
            var dataSet = new DataSet();

            // Act
            dataSet.AddPoint(point);

            // Assert
            dataSet.Data.Should().HaveCount(1).And
                .Contain(point, "при добавлении точки она должна содержаться в множестве");
        }

        [Fact]
        public void DataSetShouldAddMultipleElements()
        {
            // Arrange
            IEnumerable<DataPoint> enumerable = new List<DataPoint>
            {
                A.Fake<DataPoint>(),
                A.Fake<DataPoint>(),
                A.Fake<DataPoint>(),
            };
            var dataSet = new DataSet();

            // Act
            dataSet.AddPoints(enumerable);

            // Assert
            dataSet.Data.Should().BeEquivalentTo(enumerable);
        }

        [Fact]
        public void DataSetShouldDetermineIfItHasElement()
        {
            // Arrange
            var pointIn = A.Fake<DataPoint>();
            var pointOut = A.Fake<DataPoint>();
            var dataSet = new DataSet();

            // Act
            dataSet.AddPoint(pointIn);

            // Assert
            dataSet.HasPoint(pointIn).Should().BeTrue();
            dataSet.HasPoint(pointOut).Should().BeFalse();
        }
    }
}