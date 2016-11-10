using System.Collections.Generic;
using GalaSoft.MvvmLight;
using ClusterMongo;
using ClusterDomain;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;
using System.Linq;

namespace Clusters.ViewModel
{
    public class DatabaseViewModel : ViewModelBase
    {
        public DatabaseViewModel(DBContext dataProvider)
        {
            this.dataProvider = dataProvider;
            LoadDataSets();
            SelectedDataSet = null;
            SelectedIdx = -1;
        }

        #region Properties

        private IEnumerable<IDataSet> loadedDataSets;
        public IEnumerable<IDataSet> LoadedDataSets
        {
            get
            {
                return loadedDataSets;
            }
            set
            {
                if (loadedDataSets != value)
                {
                    loadedDataSets = value;
                    RaisePropertyChanged(nameof(LoadedDataSets));
                }
            }
        }

        public IDataSet SelectedDataSet
        {
            get;
            set;
        }

        private int selectedIdx;
        public int SelectedIdx
        {
            get
            {
                return selectedIdx;
            }
            set
            {
                if (selectedIdx != value)
                {
                    selectedIdx = value;
                    RaisePropertyChanged(nameof(SelectedIdx));
                }
            }
        }

        #endregion


        #region Private Methods

        private void LoadDataSets()
        {
            LoadedDataSets = dataProvider.GetDataSets();
        }

        #endregion

        #region Commands

        private RelayCommand<Window> loadDataSetCommand;

        public RelayCommand<Window> LoadDataSetCommand
        {
            get
            {
                if (loadDataSetCommand == null)
                {
                    loadDataSetCommand = new RelayCommand<Window>(MarkSelectedDataSet);
                }
                return loadDataSetCommand;
            }
        }

        private RelayCommand removeDataSetCommand;

        public RelayCommand RemoveDataSetCommand
        {
            get
            {
                if (removeDataSetCommand == null)
                {
                    removeDataSetCommand = new RelayCommand(RemoveSelectedDataSet);
                }
                return removeDataSetCommand;
            }
        }

        #endregion

        #region Command Methods

        private void MarkSelectedDataSet(Window w)
        {
            if (selectedIdx != -1)
            {
                SelectedDataSet = LoadedDataSets.ElementAt(selectedIdx);
                if (w != null)
                {
                    w.Close();
                }
            }
        }

        private void RemoveSelectedDataSet()
        {
            if (selectedIdx != -1)
            {
                dataProvider.DeleteDataSet(LoadedDataSets.ElementAt(selectedIdx).Name);
                LoadDataSets();
            }
        }

        #endregion

        #region Private Fields

        DBContext dataProvider;

        #endregion
    }
}
