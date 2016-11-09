using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Ninject;
using ClusterDomain;

namespace Clusters
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IKernel Container { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            this.Container = new StandardKernel(new ClusterMongo.NinjectDatabaseContextModule());
            Container.Bind<IDataSetFactory>().To<DataSetFactory>().InTransientScope();
            Container.Bind<IClusterizerBuilder>().To<DbscanClusterizerBuilder>().InTransientScope();
            Container.Bind<DataSetRepository>().To<ClusterMongo.DBRepository>();
        }

        private void ComposeObjects()
        {
            Current.MainWindow = this.Container.Get<Views.MainView>();
            Current.MainWindow.DataContext = this.Container.Get<ViewModel.MainViewModel>();
        }
    }


}
