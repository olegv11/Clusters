using System.Collections.Generic;
using System.Linq;
using ClusterDomain;
using ClusterMongo;
using Xunit;
using FakeItEasy;
using System;
using FluentAssertions;

namespace ClusterMongoTest
{
    public class MongoDBRepositoryTest
    {
        [Fact]
        public void TestMongoDBRepositoryShouldBeCreated()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            // Act

            Action doCallConstructorRight = () => new DBRepository(context);

            // Assert

            doCallConstructorRight.ShouldNotThrow("корректный вызов конструктора");
        }

        [Fact]
        public void TestMongoDBRepositoryShouldThrowExceptionOnConstructorWithNullAsParameter()
        {
            // Act

            Action doCallConstructorWrong = () => new DBRepository(null);

            // Assert

            doCallConstructorWrong.ShouldThrow<ArgumentException>().WithMessage("Некорректный параметр конструктора");
        }


        [Fact]
        public void TestMongoDBRepositoryShouldGetAllDataSets()
        {
            // Arrange
            
            var context = A.Fake<DBContext>();

            var list = new List<DataSetInterface>
            {
                A.Fake<DataSetInterface>(),
                A.Fake<DataSetInterface>(),
                A.Fake<DataSetInterface>()
            }.AsEnumerable();

            A.CallTo(() => context.DataSetAsEnumerable()).Returns(list);

            DBRepository repo = new DBRepository(context);

            // Act
            var result = repo.GetAllDataSets();

            // Assert

            result.Should().HaveCount(3, "должно быть 3 набора данных.");
        }

        [Fact]
        public void TestMongoDBRepositoryShouldGetAllDataSetsIfEmpty()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            var list = new List<DataSetInterface>{ }.AsEnumerable();
            A.CallTo(() => context.DataSetAsEnumerable()).Returns(list);

            DBRepository repo = new DBRepository(context);

            // Act
            var result = repo.GetAllDataSets();

            // Assert

            result.Should().HaveCount(0, "должно быть 0 наборов данных.");
        }

        [Fact]
        public void TestMongoDBRepositoryShouldGetDataSetByName()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            var dataSet1 = A.Fake<DataSetInterface>();
            A.CallTo(() => dataSet1.Name).Returns("DataSet1");
            var dataSet2 = A.Fake<DataSetInterface>();
            A.CallTo(() => dataSet2.Name).Returns("DataSet2");
            var dataSet3 = A.Fake<DataSetInterface>();
            A.CallTo(() => dataSet3.Name).Returns("DataSet3");

            var list = new List<DataSetInterface>
            {
                dataSet1,
                dataSet2,
                dataSet3
            }.AsEnumerable();

            A.CallTo(() => context.DataSetAsEnumerable()).Returns(list);

            DBRepository repo = new DBRepository(context);

            // Act
            var result = repo.GetDataSetByName("DataSet2");

            // Assert

            result.Name.ShouldBeEquivalentTo("DataSet2", "не найден искомый набор данных");

        }

        [Fact]
        public void TestMongoDBRepositoryShouldThrowAnExceptionIfNoFoundOnGetDataSetByName()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            var dataSet1 = A.Fake<DataSetInterface>();
            A.CallTo(() => dataSet1.Name).Returns("DataSet1");
            var dataSet2 = A.Fake<DataSetInterface>();
            A.CallTo(() => dataSet2.Name).Returns("DataSet2");
            var dataSet3 = A.Fake<DataSetInterface>();
            A.CallTo(() => dataSet3.Name).Returns("DataSet3");

            var list = new List<DataSetInterface>
            {
                dataSet1,
                dataSet2,
                dataSet3
            }.AsEnumerable();

            A.CallTo(() => context.DataSetAsEnumerable()).Returns(list);

            DBRepository repo = new DBRepository(context);

            Action doGetDataSetByName = () => repo.GetDataSetByName("DataSet4");

            // Assert
            doGetDataSetByName.ShouldThrow<MongoDBException>().WithMessage("Не найдено подходящего набора данных");
        }

        [Fact]
        public void TestMongoDBRepositoryShouldThrowAnExceptionIfMultipleFoundOnGetDataSetByName()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            var dataSet1 = A.Fake<DataSetInterface>();
            A.CallTo(() => dataSet1.Name).Returns("DataSet1");
            var dataSet2 = A.Fake<DataSetInterface>();
            A.CallTo(() => dataSet2.Name).Returns("DataSet1");
            var dataSet3 = A.Fake<DataSetInterface>();
            A.CallTo(() => dataSet3.Name).Returns("DataSet3");

            var list = new List<DataSetInterface>
            {
                dataSet1,
                dataSet2,
                dataSet3
            }.AsEnumerable();

            A.CallTo(() => context.DataSetAsEnumerable()).Returns(list);

            DBRepository repo = new DBRepository(context);

            Action doGetDataSetByName = () => repo.GetDataSetByName("DataSet1");

            // Assert
            doGetDataSetByName.ShouldThrow<MongoDBException>().WithMessage("Найден повтор в базе данных");
        }

        [Fact]
        public void TestMongoDBRepositorySaveDataSetShouldCallContextSaveDataSetFunction()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            var dataSet1 = A.Fake<DataSetInterface>();
            A.CallTo(() => dataSet1.Name).Returns("DataSet1");
            A.CallTo(() => context.SaveDataSet(dataSet1)).DoesNothing();
            DBRepository repo = new DBRepository(context);

            repo.SaveDataSet(dataSet1);

            //Assert
            A.CallTo(() => context.SaveDataSet(A<DataSetInterface>.That.Matches(ds => 
                ds == dataSet1))).MustHaveHappened();
        }

        [Fact]
        public void TestMongoDBRepositoryDeleteDataSetShouldCallContextDeleteDataSetFunction()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            var dataSet1 = A.Fake<DataSetInterface>();
            A.CallTo(() => dataSet1.Name).Returns("DataSet1");
            A.CallTo(() => context.DeleteDataSet(dataSet1.Name)).DoesNothing();
            DBRepository repo = new DBRepository(context);

            repo.DeleteDataSet(dataSet1.Name);

            //Assert
            A.CallTo(() => context.DeleteDataSet(A<string>.That.Matches(name =>
                name == dataSet1.Name))).MustHaveHappened();
        }
    }
}
