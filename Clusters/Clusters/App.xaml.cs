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
        private IKernel container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            this.container = new StandardKernel();
            container.Bind<IDataSetFactory>().To<DataSetFactory>().InTransientScope();
            container.Bind<IClusterizerBuilder>().To<DbscanClusterizerBuilder>().InTransientScope();
        }

        private void ComposeObjects()
        {
            Current.MainWindow = this.container.Get<Views.MainView>();
            Current.MainWindow.DataContext = this.container.Get<ViewModel.MainViewModel>();
        }
    }


}
