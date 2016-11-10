using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Clusters;
using ClusterMongo;
using ClusterDomain;
using Ninject;
using System.Windows;
using MongoDB.Driver;

namespace ClustersUITest
{
    public class DBVMIntegration : IDisposable
    {

        [WpfFact]
        public void DBVMShoulBeAbleToReadDataBase()
        {

            //Arrange
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

            IKernel kernel = new StandardKernel(new NinjectTestDatabaseContextModule());

            kernel.Bind<IDataSetFactory>().To<DataSetFactory>().InTransientScope();
            kernel.Bind<IClusterizerBuilder>().To<DbscanClusterizerBuilder>().InTransientScope();
            kernel.Bind<DataSetRepository>().To<ClusterMongo.DBRepository>();

            var dataFactory = kernel.Get<IDataSetFactory>();
            var repo = kernel.Get<DBRepository>();
            var megaCluster = dataFactory.DataSetFromIEnumarableOfPoints(cluster1.Union(cluster2).Union(noise));

            repo.SaveDataSet(megaCluster);

            megaCluster = dataFactory.DataSetFromIEnumarableOfPoints(cluster1.Union(cluster2));

            repo.SaveDataSet(megaCluster);



            //Act
            var dbvm = kernel.Get<Clusters.ViewModel.DatabaseViewModel>();

            //Assert
            dbvm.LoadedDataSets.Count().ShouldBeEquivalentTo(2, "Загрузили все данные");

        }

        [WpfFact]
        public void DBVMShoulBeAbleToRemoveDataSet()
        {

            //Arrange
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

            IKernel kernel = new StandardKernel(new NinjectTestDatabaseContextModule());

            kernel.Bind<IDataSetFactory>().To<DataSetFactory>().InTransientScope();
            kernel.Bind<IClusterizerBuilder>().To<DbscanClusterizerBuilder>().InTransientScope();
            kernel.Bind<DataSetRepository>().To<ClusterMongo.DBRepository>();

            var dataFactory = kernel.Get<IDataSetFactory>();
            var repo = kernel.Get<DBRepository>();
            var megaCluster = dataFactory.DataSetFromIEnumarableOfPoints(cluster1.Union(cluster2).Union(noise));

            repo.SaveDataSet(megaCluster);

            megaCluster = dataFactory.DataSetFromIEnumarableOfPoints(cluster1.Union(cluster2));

            repo.SaveDataSet(megaCluster);

            var dbvm = kernel.Get<Clusters.ViewModel.DatabaseViewModel>();

            dbvm.MonitorEvents();


            //Act
            dbvm.SelectedIdx = 0;
            dbvm.RemoveDataSetCommand.Execute(null);

            //Assert
            dbvm.ShouldRaisePropertyChangeFor(x => x.LoadedDataSets);
            dbvm.LoadedDataSets.Count().ShouldBeEquivalentTo(1, "Остался 1 сет");
            dbvm.LoadedDataSets.ElementAt(0).Data.Should().HaveSameCount(megaCluster.Data, "Это второй сет");
            dbvm.LoadedDataSets.ElementAt(0).Data.Should().Match(x => x.SequenceEqual(megaCluster.Data), "Это точно второй сет");
        }


        public DBVMIntegration()
        {
            var client = new MongoClient(@"mongodb://localhost:27017");
            var Db = client.GetDatabase("ClustersTestDB");
            Db.GetCollection<MongoSet>("DataSets").DeleteMany(x => true);
        }
        public void Dispose()
        {
            var client = new MongoClient(@"mongodb://localhost:27017");
            var Db = client.GetDatabase("ClustersTestDB");
            Db.GetCollection<MongoSet>("DataSets").DeleteMany(x => true);
        }
    }
}
