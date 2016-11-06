using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using ClusterDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;

namespace AcceptanceTests
{
    [Binding]
    public class ClusterizationSteps
    {
        private UiMap map;

        private List<Tuple<double, double>> inputPoints;
        private List<List<Tuple<double, double>>> resultClusters;
        private List<Tuple<double, double>> resultNoise;

        [Given(@"I started application")]
        public void GivenIStartedApplication()
        {
            var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            path = Path.Combine(path, @"Clusters\bin\Debug\", "Clusters.exe");

            map = new UiMap(path);
        }

        [Given(@"I entered points \{(.*)\}")]
        public void GivenIEnteredPoints(string points)
        {
            inputPoints = GetListOfPoints(points);
            foreach (var p in inputPoints)
            {
                map.EnterPX(p.Item1);
                map.EnterPY(p.Item2);
                map.PressAddPoint();
                map.WaitWhileBusy();
            }
        }
        
        [Given(@"I entered minimal number of points (.*)")]
        public void GivenIEnteredMinimalNumberOfPoints(int minPoints)
        {
            map.EnterMinPoints(minPoints);
        }
        
        [Given(@"I entered epsilon (.*)")]
        public void GivenIEnteredEpsilon(double eps)
        {
            map.EnterEps(eps);
        }
        
        [Given(@"I chose metric LP\((.*)\)")]
        public void GivenIChoseMetricLP(double p)
        {
            map.ChooseLpMetric();
            map.EnterP(p);
        }

        [Given(@"I chose metric Sup")]
        public void GivenIChoseMetricSup()
        {
            map.ChooseSupMetric();
        }

        [When(@"I pressed clusterize")]
        public void WhenIPressedClusterize()
        {
            map.PressClusterize();
        }
        
        [Then(@"There must be clusters \[(.*)] and noise \{(.*)\}")]
        public void ThenThereMustBeClustersAndNoise(string clusterString, string noiseString)
        {
            resultClusters = GetListOfClusters(clusterString).Select(GetListOfPoints).ToList();
            resultNoise = GetListOfPoints(noiseString);
            Thread.Sleep(10000);
        }

        public List<string> GetListOfClusters(string s)
        {
            var result = new List<string>();
            Regex r = new Regex(@"\{(.*?)\}");
            var m = r.Matches(s);

            for (int i = 0; i < m.Count; i++)
            {
                result.Add(m[i].Groups[1].Value);
            }

            return result;
        }

        public List<Tuple<double, double>> GetListOfPoints(string s)
        {
            var result = new List<Tuple<double, double>>();

            var separated = s.Split(',');
            Regex r = new Regex(@"\((.*);(.*)\)");
            foreach (var pointString in separated)
            {
                var m = r.Matches(pointString);
                var x = double.Parse(m[0].Groups[1].Value);
                var y = double.Parse(m[0].Groups[2].Value);
                result.Add(new Tuple<double, double>(x, y));
            }

            return result;
        }

        [AfterScenario]
        public void CloseApplication()
        {
            map.Close();
        }
    }
}
