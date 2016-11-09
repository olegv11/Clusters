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
    public class MainVMIntegration : IDisposable
    {
        [WpfFact]
        public void UIShouldBeAbleToSaveDataInDatabase()
        {
            IKernel kernel = new StandardKernel(new NinjectTestDatabaseContextModule());

            kernel.Bind<IDataSetFactory>().To<DataSetFactory>().InTransientScope();
            kernel.Bind<IClusterizerBuilder>().To<DbscanClusterizerBuilder>().InTransientScope();
            kernel.Bind<DataSetRepository>().To<ClusterMongo.DBRepository>();

            var mvm = kernel.Get<Clusters.ViewModel.MainViewModel>();
            var repo = kernel.Get<DBRepository>();

            mvm.XInput = 1;
            mvm.YInput = 1;

            mvm.AddPointCommand.Execute(null);


            Action result = () => mvm.SaveToDB.Execute(null);

            result.ShouldNotThrow("Есть соединение с бд");
            repo.GetAllDataSets().Count().ShouldBeEquivalentTo(1, "Единственная запись");
        }

        [WpfFact]
        public void UIShoulBeAbleToUseClusterizerInSuperemumNormTest()
        {
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

            var megaCLuster = cluster1.Union(cluster2).Union(noise).ToList();

            IKernel kernel = new StandardKernel(new NinjectTestDatabaseContextModule());

            kernel.Bind<IDataSetFactory>().To<DataSetFactory>().InTransientScope();
            kernel.Bind<IClusterizerBuilder>().To<DbscanClusterizerBuilder>().InTransientScope();
            kernel.Bind<DataSetRepository>().To<ClusterMongo.DBRepository>();

            var mvm = kernel.Get<Clusters.ViewModel.MainViewModel>();

            foreach(var point in megaCLuster)
            {
                mvm.XInput = point.X;
                mvm.YInput = point.Y;
                mvm.AddPointCommand.Execute(null);
            }

            mvm.SelectedMetricIndex = 1;

            Action result = () => mvm.ClusteriseCommand.Execute(null);

            result.Invoke();
            mvm.ClusteredData.Count.ShouldBeEquivalentTo(3, "Разбили не 3 кластера");
            mvm.ClusteredData[0].Values.Count.ShouldBeEquivalentTo(5, "5 в 1 кластере");
            mvm.ClusteredData[1].Values.Count.ShouldBeEquivalentTo(5, "5 в 2 кластере");
            mvm.ClusteredData[2].Values.Count.ShouldBeEquivalentTo(3, "3 в кластере шума");
        }


        public MainVMIntegration()
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
