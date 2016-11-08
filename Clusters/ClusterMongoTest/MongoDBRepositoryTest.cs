﻿using ClusterDomain;
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
        public void MongoDBRepositoryShouldBeCreated()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            // Act

            Action doCallConstructorRight = () => new DBRepository(context);

            // Assert

            doCallConstructorRight.ShouldNotThrow("корректный вызов конструктора");
        }

        [Fact]
        public void MongoDBRepositoryShouldThrowExceptionOnConstructorWithNullAsParameter()
        {
            // Act

            Action doCallConstructorWrong = () => new DBRepository(null);

            // Assert

            doCallConstructorWrong.ShouldThrow<ArgumentException>().WithMessage("Некорректный параметр конструктора");
        }


        [Fact]
        public void MongoDBRepositoryShouldGetAllDataSets()
        {
            // Arrange
            
            var context = A.Fake<DBContext>();

            var list = new IDataSet[]
            {
                A.Fake<IDataSet>(),
                A.Fake<IDataSet>(),
                A.Fake<IDataSet>()
            };

            A.CallTo(() => context.GetDataSets()).Returns(list);

            DBRepository repo = new DBRepository(context);

            // Act
            var result = repo.GetAllDataSets();

            // Assert

            result.Should().BeEquivalentTo(list, "должны быть равны все элементы.");
        }

        [Fact]
        public void MongoDBRepositoryShouldGetAllDataSetsIfEmpty()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            var list = new IDataSet[] {};
            A.CallTo(() => context.GetDataSets()).Returns(list);

            DBRepository repo = new DBRepository(context);

            // Act
            var result = repo.GetAllDataSets();

            // Assert

            result.Should().BeEmpty("должно быть 0 наборов данных.");
        }

        [Fact]
        public void MongoDBRepositoryShouldGetAllDataSetsIfNull()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            A.CallTo(() => context.GetDataSets()).Returns(null);

            DBRepository repo = new DBRepository(context);

            // Act
            var result = repo.GetAllDataSets();

            // Assert

            result.Should().BeNull("должен быть null");
        }

        [Fact]
        public void MongoDBRepositoryShouldGetDataSetByName()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            var dataSet1 = A.Fake<IDataSet>();
            A.CallTo(() => dataSet1.Name).Returns("DataSet1");
            var dataSet2 = A.Fake<IDataSet>();
            A.CallTo(() => dataSet2.Name).Returns("DataSet2");
            var dataSet3 = A.Fake<IDataSet>();
            A.CallTo(() => dataSet3.Name).Returns("DataSet3");

            var list = new IDataSet[]
            {
                dataSet1,
                dataSet2,
                dataSet3
            };

            A.CallTo(() => context.GetDataSets()).Returns(list);

            DBRepository repo = new DBRepository(context);

            // Act
            var result = repo.GetDataSetByName("DataSet2");

            // Assert

            result.Name.ShouldBeEquivalentTo("DataSet2", "не найден искомый набор данных");

        }

        [Fact]
        public void MongoDBRepositoryShouldThrowAnExceptionIfNoFoundOnGetDataSetByName()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            var dataSet1 = A.Fake<IDataSet>();
            A.CallTo(() => dataSet1.Name).Returns("DataSet1");
            var dataSet2 = A.Fake<IDataSet>();
            A.CallTo(() => dataSet2.Name).Returns("DataSet2");
            var dataSet3 = A.Fake<IDataSet>();
            A.CallTo(() => dataSet3.Name).Returns("DataSet3");

            var list = new IDataSet[]
            {
                dataSet1,
                dataSet2,
                dataSet3
            };

            A.CallTo(() => context.GetDataSets()).Returns(list);

            DBRepository repo = new DBRepository(context);

            Action doGetDataSetByName = () => repo.GetDataSetByName("DataSet4");

            // Assert
            doGetDataSetByName.ShouldThrow<MongoDBException>().WithMessage("Не найдено подходящего набора данных");
        }
        [Fact]
        public void MongoDBRepositoryShouldThrowAnExceptionIfNoNullPassedAsArgumentOnGetDataSetByName()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            var dataSet1 = A.Fake<IDataSet>();
            A.CallTo(() => dataSet1.Name).Returns("DataSet1");
            var dataSet2 = A.Fake<IDataSet>();
            A.CallTo(() => dataSet2.Name).Returns("DataSet2");
            var dataSet3 = A.Fake<IDataSet>();
            A.CallTo(() => dataSet3.Name).Returns("DataSet3");

            var list = new IDataSet[]
            {
                dataSet1,
                dataSet2,
                dataSet3
            };

            A.CallTo(() => context.GetDataSets()).Returns(list);

            DBRepository repo = new DBRepository(context);

            Action doGetDataSetByName = () => repo.GetDataSetByName(null);

            // Assert
            doGetDataSetByName.ShouldThrow<ArgumentNullException>("передан null в качестве параметра");
        }

        [Fact]
        public void MongoDBRepositoryShouldThrowAnExceptionIfMultipleFoundOnGetDataSetByName()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            var dataSet1 = A.Fake<IDataSet>();
            A.CallTo(() => dataSet1.Name).Returns("DataSet1");
            var dataSet2 = A.Fake<IDataSet>();
            A.CallTo(() => dataSet2.Name).Returns("DataSet2");
            var dataSet3 = A.Fake<IDataSet>();
            A.CallTo(() => dataSet3.Name).Returns("DataSet3");

            var list = new IDataSet[]
            {
                dataSet1,
                dataSet2,
                dataSet3,
                dataSet1
            };

            A.CallTo(() => context.GetDataSets()).Returns(list);

            DBRepository repo = new DBRepository(context);

            Action doGetDataSetByName = () => repo.GetDataSetByName("DataSet1");

            // Assert
            doGetDataSetByName.ShouldThrow<MongoDBException>().WithMessage("Найден повтор в базе данных");
        }

        [Fact]
        public void MongoDBRepositorySaveDataSetShouldCallContextSaveDataSetFunction()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            var dataSet1 = A.Fake<IDataSet>();
            A.CallTo(() => dataSet1.Name).Returns("DataSet1");
            A.CallTo(() => context.SaveDataSet(dataSet1)).DoesNothing();
            DBRepository repo = new DBRepository(context);

            // Act
            repo.SaveDataSet(dataSet1);

            // Assert
            A.CallTo(() => context.SaveDataSet(A<IDataSet>.That.Matches(ds => 
                ds == dataSet1))).MustHaveHappened();
        }

        [Fact]
        public void MongoDBRepositorySaveDataSetShouldThrowAnExceptionIfArgumentIsNull()
        {
            // Arrange

            var context = A.Fake<DBContext>();
            A.CallTo(() => context.SaveDataSet(null)).DoesNothing();
            DBRepository repo = new DBRepository(context);

            Action act = () => repo.SaveDataSet(null);

            //Assert
            act.ShouldThrow<ArgumentNullException>("передан null в качестве аргумента.");
        }

        [Fact]
        public void MongoDBRepositoryDeleteDataSetShouldCallContextDeleteDataSetFunction()
        {
            // Arrange

            var context = A.Fake<DBContext>();

            var dataSet1 = A.Fake<IDataSet>();
            A.CallTo(() => dataSet1.Name).Returns("DataSet1");
            A.CallTo(() => context.DeleteDataSet(dataSet1.Name)).DoesNothing();
            DBRepository repo = new DBRepository(context);

            // Act
            repo.DeleteDataSet(dataSet1.Name);

            // Assert
            A.CallTo(() => context.DeleteDataSet(A<string>.That.Matches(name =>
                name == dataSet1.Name))).MustHaveHappened();
        }

        [Fact]
        public void MongoDBRepositoryDeleteDataSetShouldThrowAnExceptionIfArgumentIsNull()
        {
            // Arrange

            var context = A.Fake<DBContext>();
            A.CallTo(() => context.DeleteDataSet(null)).DoesNothing();
            DBRepository repo = new DBRepository(context);

            Action act = () => repo.DeleteDataSet(null);

            //Assert
            act.ShouldThrow<ArgumentNullException>("передан null в качестве аргумента.");
        }
    }
}
