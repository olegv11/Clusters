using ClusterDomain;
using ClusterMongo;
using Xunit;
using FakeItEasy;
using System;
using System.Linq;
using FluentAssertions;
using Ninject;
using System.Collections.Generic;

namespace ClusterMongoTest
{
    public class MongoDBIntegration
    {
        [Fact]
        public void MongoDBRepositoryShouldBeCreated()
        {
            // Arrange
            IKernel kernel = new StandardKernel(new NinjectDatabaseContextModule());
            // Act
            Action result = () => kernel.Get<DBRepository>();
            // Assert
            result.ShouldNotThrow("корректное подключение");
        }

        [Fact]
        public void MongoDBRepositoryShouldAddAndNewEntry()
        {
            // Arrange
            IKernel kernel = new StandardKernel(new NinjectDatabaseContextModule());
            DBRepository repo = kernel.Get<DBRepository>();

            // Let's hope that it works 
            repo.DeleteAllDataSets();

            var dataSet1 = new DataSet();
            dataSet1.Name = "DataSet1";
            
            // Act
            Action result = () => repo.SaveDataSet(dataSet1);
            // Assert
            result.ShouldNotThrow("корректное добавление записи");
        }

        [Fact]
        public void MongoDBRepositoryShouldGetAddedEntry()
        {
            // Arrange
            IKernel kernel = new StandardKernel(new NinjectDatabaseContextModule());
            DBRepository repo = kernel.Get<DBRepository>();

            var dataSet1 = new DataSet();
            dataSet1.Name = "DataSet1";

            // Let's hope that it works
            repo.DeleteAllDataSets();

            repo.SaveDataSet(dataSet1);

            // Act
            Action result = () => repo.GetDataSetByName(dataSet1.Name);

            // Assert
            result.ShouldNotThrow("корректное добавление записи");
            repo.GetDataSetByName(dataSet1.Name).Name.ShouldBeEquivalentTo(dataSet1.Name, "записи должны совпадать");
        }

        [Fact]
        public void MongoDBRepositoryShouldThrowAnExceptionWhenGettingNonexistantDataSet()
        {
            // Arrange
            IKernel kernel = new StandardKernel(new NinjectDatabaseContextModule());
            DBRepository repo = kernel.Get<DBRepository>();

            var dataSet1 = new DataSet();
            dataSet1.Name = "DataSet1";

            // Let's hope that it works
            repo.DeleteAllDataSets();

            repo.SaveDataSet(dataSet1);

            // Act
            Action result = () => repo.GetDataSetByName("DataSetABABA");

            // Assert
            result.ShouldThrow<MongoDBException>().WithMessage("Не найдено подходящего набора данных", "запись отсутсвует в БД");
        }

        [Fact]
        public void MongoDBRepositoryShouldDeleteAllEntries()
        {
            // Arrange
            IKernel kernel = new StandardKernel(new NinjectDatabaseContextModule());
            DBRepository repo = kernel.Get<DBRepository>();

            repo.DeleteAllDataSets();

            var dataSet1 = new DataSet();
            dataSet1.Name = "DataSet1";
            var dataSet2 = new DataSet();
            dataSet2.Name = "DataSet2";
            var dataSet3 = new DataSet();
            dataSet3.Name = "DataSet3";

            repo.SaveDataSet(dataSet1);
            repo.SaveDataSet(dataSet2);
            repo.SaveDataSet(dataSet3);

            // Act
            Action result = () => repo.DeleteAllDataSets();

            // Assert
            result.ShouldNotThrow("корректное удаление записей");
            repo.GetAllDataSets().Count().ShouldBeEquivalentTo(0, "все записи удалены");
        }

        [Fact]
        public void MongoDBRepositoryShouldUpdateAnEntry()
        {
            // Arrange
            IKernel kernel = new StandardKernel(new NinjectDatabaseContextModule());
            DBRepository repo = kernel.Get<DBRepository>();

            repo.DeleteAllDataSets();

            var dataSet1 = new DataSet();
            dataSet1.Name = "DataSet1";
            dataSet1.CreationTime = DateTime.MaxValue;

            repo.SaveDataSet(dataSet1);

            dataSet1.CreationTime = DateTime.MinValue;
            // Act
            Action result = () => repo.SaveDataSet(dataSet1);

            // Assert
            
            result.ShouldNotThrow("обновление должно быть успешно");
            var r = repo.GetDataSetByName(dataSet1.Name);
            repo.GetDataSetByName(dataSet1.Name).CreationTime.ShouldBeEquivalentTo(dataSet1.CreationTime, "данное поле должно быть обновлено");
        }

        [Fact]
        public void MongoDBRepositoryShouldDeleteAnAddedEntry()
        {
            // Arrange
            IKernel kernel = new StandardKernel(new NinjectDatabaseContextModule());
            DBRepository repo = kernel.Get<DBRepository>();

            repo.DeleteAllDataSets();

            var dataSet1 = new DataSet();
            dataSet1.Name = "DataSet1";

            repo.SaveDataSet(dataSet1);
            
            // Act
            Action result = () => repo.DeleteDataSet("DataSet1");

            // Assert

            result.ShouldNotThrow("удаление должно быть успешно");
            repo.GetAllDataSets().Where(p => p.Name == dataSet1.Name).Count().ShouldBeEquivalentTo(0, "данная запись должна быть удалена");
        }

        [Fact]
        public void MongoDBRepositoryShouldThrowOnAttemptToDeleteNonexistantEntry()
        {
            // Arrange
            IKernel kernel = new StandardKernel(new NinjectDatabaseContextModule());
            DBRepository repo = kernel.Get<DBRepository>();

            repo.DeleteAllDataSets();

            var dataSet1 = new DataSet();
            dataSet1.Name = "DataSet1";

            repo.SaveDataSet(dataSet1);

            // Act
            Action result = () => repo.DeleteDataSet("DataSet2");

            // Assert

            result.ShouldThrow<MongoDBException>().WithMessage("Не найдено подходящего набора данных", "удаляемая запись отсутствует");
        }
    }
}
