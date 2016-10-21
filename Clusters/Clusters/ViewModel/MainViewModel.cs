using ClusterDomain;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Linq;

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
        public MainViewModel()
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
        }

        #region Properties

        public List<string> Metics { get; } = new List<string> { "p-норма", "супремум-норма" };

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

        #endregion

        #region Command Mathods

        private void AddPoint()
        {
            points.Add(new DataPoint(XInput, YInput));
            ClusteredData = SeriesCollectionFromIEnumerable(points);
        }


        private void Clusterise()
        {
            var data = new DataSet(points);
            var clusterizer = new DbscanClasterizer(5, 3);
            clusterizer.Clusterize(MetricForIndex(selectedMetricIndex), data);

            var clustered_data = clusterizer.GetClusters();


            var clusters = SeriesCollectionFromIEnumerable(clustered_data.Select(x => x.Points));

            var noise = ScatterSeriesFromIEnumerable(clusterizer.GetNoise().Points);

            clusters.Add(noise);

            ClusteredData = clusters;   
        }

        #endregion

        #region Private Methods

        private SeriesCollection SeriesCollectionFromIEnumerable(IEnumerable<IEnumerable<DataPoint>> src)
        {
            var collection = new SeriesCollection();
            foreach(var cluster in src)
            {
                collection.Add(ScatterSeriesFromIEnumerable(cluster));
            }
            return collection;
        }

        private SeriesCollection SeriesCollectionFromIEnumerable(IEnumerable<DataPoint> src)
        {
            var collection = new SeriesCollection();
            collection.Add(ScatterSeriesFromIEnumerable(src));
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

        #endregion


        #region Private Fields

        private List<DataPoint> points;

        #endregion
    }
}