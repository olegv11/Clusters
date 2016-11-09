using ClusterDomain;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;

namespace Clusters.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataSetFactory dataSetFactory, IClusterizerBuilder clusterizerBuilder, DataSetRepository dataProvider)
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            points = new List<DataPoint>();
            SelectedMetricIndex = 0;
            MetricParameterValue = 2;
            SetMetricParameterVisibility();
            MPInput = 3;
            EpsInput = 5;
            DataSetName = string.Empty;
            this.dataSetFactory = dataSetFactory;
            this.clusterizerBuilder = clusterizerBuilder;
            this.dataProvider = dataProvider;
        }

        #region Properties

        private string dataSetName;

        public string DataSetName
        {
            get
            {
                return dataSetName;
            }
            set
            {
                if (dataSetName != value)
                {
                    dataSetName = value;
                    RaisePropertyChanged(nameof(DataSetName));
                }
            }
        }

        private double epsInput;

        public double EpsInput
        {
            get { return epsInput; }
            set
            {
                if (epsInput != value)
                {
                    epsInput = value;
                    RaisePropertyChanged(nameof(EpsInput));
                }
            }
        }

        private int mpInput;

        public int MPInput
        {
            get { return mpInput; }
            set
            {
                if (mpInput != value)
                {
                    mpInput = value;
                    UpdateCanClusterise();
                    RaisePropertyChanged(nameof(MPInput));
                }
            }
        }

        public bool canClusterise;

        public bool CanClusterise
        {
            get { return canClusterise; }
            set
            {
                if (canClusterise != value)
                {
                    canClusterise = value;
                    RaisePropertyChanged(nameof(CanClusterise));
                }
            }
        }

        public List<string> Metics { get; } = new List<string> { "p-метрика", "супремум-метрика" };

        private int selectedMetricIndex;
        public int SelectedMetricIndex
        {
            get { return selectedMetricIndex; }
            set
            {
                if (selectedMetricIndex != value)
                {
                    selectedMetricIndex = value;
                    SetMetricParameterVisibility();
                    RaisePropertyChanged(nameof(SelectedMetricIndex));
                }
            }
        }


        private SeriesCollection clusteredData;

        public SeriesCollection ClusteredData
        {
            get { return clusteredData; }
            set
            {
                if (clusteredData != value)
                {
                    clusteredData = value;
                    RaisePropertyChanged(nameof(ClusteredData));
                }
            }
        }

        private double xInput;
        public double XInput
        {
            get { return xInput; }
            set
            {
                if (xInput != value)
                {
                    xInput = value;
                    RaisePropertyChanged(nameof(XInput));
                }
            }
        }

        private double yInput;
        public double YInput
        {
            get { return yInput; }
            set
            {
                if (yInput != value)
                {
                    yInput = value;
                    RaisePropertyChanged(nameof(YInput));
                }
            }
        }

        private int metricParameterValue;
        public int MetricParameterValue
        {
            get { return metricParameterValue; }
            set
            {
                if (metricParameterValue != value)
                {
                    metricParameterValue = value;
                    RaisePropertyChanged(nameof(MetricParameterValue));
                }
            }
        }

        private bool metricParameterShuoldBeVisible;
        public bool MetricParameterShuoldBeVisible
        {
            get { return metricParameterShuoldBeVisible; }
            set
            {
                if (metricParameterShuoldBeVisible != value)
                {
                    metricParameterShuoldBeVisible = value;
                    RaisePropertyChanged(nameof(MetricParameterShuoldBeVisible));
                }
            }
        }

        #endregion

        #region Commands

        private RelayCommand showDatabaseCommand;

        public RelayCommand ShowDatabaseCommand
        {
            get
            {
                if (showDatabaseCommand == null)
                {
                    showDatabaseCommand = new RelayCommand(ShowDatabase);
                }
                return showDatabaseCommand;
            }
        }

        private RelayCommand saveToDB;

        public RelayCommand SaveToDB
        {
            get
            {
                if (saveToDB == null)
                {
                    saveToDB = new RelayCommand(SavePointsToDatabase);
                }
                return saveToDB;
            }
        }

        private RelayCommand addPointCommand;
        public RelayCommand AddPointCommand
        {
            get
            {
                if (addPointCommand == null)
                {
                    addPointCommand = new RelayCommand(AddPoint);
                }
                return addPointCommand;
            }
        }

        private RelayCommand clusteriseCommand;
        public RelayCommand ClusteriseCommand
        {
            get
            {
                if (clusteriseCommand == null)
                {
                    clusteriseCommand = new RelayCommand(Clusterise);
                }
                return clusteriseCommand;
            }
        }

        private RelayCommand showresultcommand;
        public RelayCommand ShowResultCommand
        {
            get
            {
                if (showresultcommand == null)
                {
                    showresultcommand = new RelayCommand(ShowResult);
                }
                return showresultcommand;
            }
        }

        #endregion

        #region Command Methods

        private void ShowDatabase()
        {
            var dbvm = (App.Current as App).Container.Get<DatabaseViewModel>();
            var dbWindow = new Views.DatabaseView();
            dbWindow.DataContext = dbvm;
            dbWindow.ShowDialog();

            var selectedDataSet = dbvm.SelectedDataSet;

            if (selectedDataSet != null)
            {
                points = selectedDataSet.Data.ToList();

                ClusteredData = SeriesCollectionFromIEnumerable(points);
                UpdateCanClusterise();
            }
        }

        private void SavePointsToDatabase()
        {
            var set = dataSetFactory.DataSetFromIEnumarableOfPoints(points);

            if (dataSetName != string.Empty)
            {
                set.Name = dataSetName;
            }
            dataProvider.SaveDataSet(set);
        }

        private void AddPoint()
        {
            points.Add(new DataPoint(XInput, YInput));
            ClusteredData = SeriesCollectionFromIEnumerable(points);
            UpdateCanClusterise();
        }


        private void Clusterise()
        {
            var data = dataSetFactory.DataSetFromIEnumarableOfPoints(points);
            clusterizerBuilder["minPoints"] = MPInput;
            clusterizerBuilder["epsilon"] = EpsInput;
            var clusterizer = clusterizerBuilder.Build();
            clusterizer.Clusterize(MetricForIndex(selectedMetricIndex), data);

            var clustered_data = clusterizer.GetClusters();


            var clusters = SeriesCollectionFromIEnumerable(clustered_data.Select(x => x.Points));

            var noise = ScatterSeriesWithTitleFromIEnumerable(clusterizer.GetNoise().Points, "Шум");

            clusters.Add(noise);

            ClusteredData = clusters;
        }

        private void ShowResult()
        {
            string r = MakeResultString();


        }

        #endregion

        #region Private Methods

        private SeriesCollection SeriesCollectionFromIEnumerable(IEnumerable<IEnumerable<DataPoint>> src)
        {
            var collection = new SeriesCollection();
            int i = 1;
            foreach(var cluster in src)
            {
                collection.Add(ScatterSeriesWithTitleFromIEnumerable(cluster, string.Format("Кластер {0}", i)));
                i++;
            }
            return collection;
        }

        private SeriesCollection SeriesCollectionFromIEnumerable(IEnumerable<DataPoint> src)
        {
            var collection = new SeriesCollection();
            collection.Add(ScatterSeriesWithTitleFromIEnumerable(src, "Точки"));
            return collection;
        }

        private ScatterSeries ScatterSeriesFromIEnumerable(IEnumerable<DataPoint> src)
        {
            var series = new ScatterSeries();
            series.MaxPointShapeDiameter = 10;
            series.Values = new ChartValues<ScatterPoint>();
            foreach(var el in src)
            {
                series.Values.Add(new ScatterPoint(el.X, el.Y));
            }
            return series;
        }

        private ScatterSeries ScatterSeriesWithTitleFromIEnumerable(IEnumerable<DataPoint> src, string title)
        {
            var series = new ScatterSeries();
            series.Title = title;
            series.MaxPointShapeDiameter = 10;
            series.Values = new ChartValues<ScatterPoint>();
            foreach (var el in src)
            {
                series.Values.Add(new ScatterPoint(el.X, el.Y));
            }
            return series;
        }

        private Metric MetricForIndex(int idx)
        {
            Metric m = null;
            switch(idx)
            {
                case 0:
                    m = new PNorm(metricParameterValue);
                    break;
                case 1:
                    m = new SupNorm();
                    break;
            }
            return m;
        }

        private void SetMetricParameterVisibility()
        {
            switch(selectedMetricIndex)
            {
                case 0:
                    MetricParameterShuoldBeVisible = true;
                    break;
                case 1:
                    MetricParameterShuoldBeVisible = false;
                    break;
            }
        }

        private void UpdateCanClusterise()
        {
            CanClusterise = points.Count >= MPInput;
        }

        private string MakeResultString()
        {
            if (ClusteredData == null || ClusteredData.Count == 0)
            {
                return "Ничего не введено";
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ClusteredData.Count - 1; i++)
            {
                sb.Append($"Кластер {i + 1}: {{");
                foreach (ScatterPoint p in ClusteredData[i].Values)
                {
                    sb.Append($"({p.X},{p.Y})");
                }
                sb.Append("}\n");
            }

            sb.Append("Шум: {");
            foreach (ScatterPoint p in ClusteredData[ClusteredData.Count - 1].Values)
            {
                sb.Append($"({p.X},{p.Y})");
            }
            sb.Append("}\n");

            return sb.ToString();
        }

        #endregion


        #region Private Fields

        private List<DataPoint> points;
        IDataSetFactory dataSetFactory;
        IClusterizerBuilder clusterizerBuilder;
        DataSetRepository dataProvider;

        #endregion
    }
}