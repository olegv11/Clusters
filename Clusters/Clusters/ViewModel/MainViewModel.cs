using ClusterDomain;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Collections.Generic;

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
        }

        #region Properties


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


        #region Private Methods

        private void AddPoint()
        {
            points.Add(new DataPoint(XInput, YInput));
            ClusteredData = SeriesCollectionFromIEnumerable(points);
        }


        private void Clusterise()
        {

        }


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

        #endregion


        #region Private Fields

        private List<DataPoint> points;

        #endregion
    }
}