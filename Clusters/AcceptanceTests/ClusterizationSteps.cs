using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using ClusterDomain;
using FluentAssertions;
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
            inputPoints = Utility.GetListOfPoints(points);
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

        [When(@"I pressed results")]
        public void WhenIPressedResults()
        {
            map.PressResult();
        }
        
        [Then(@"There must be clusters \[(.*)] and noise \{(.*)\}")]
        public void ThenThereMustBeClustersAndNoise(string clusterString, string noiseString)
        {
            var expectedClusters = Utility.GetListOfClusters(clusterString).Select(Utility.GetListOfPoints).ToList();
            var expectedNoise = Utility.GetListOfPoints(noiseString);
            var r = map.GetClustersAndNoise();
            var resultClusters = r.Item1;
            var resultNoise = r.Item2;

            expectedNoise.ShouldBeEquivalentTo(resultNoise);
            expectedClusters.All(x => resultClusters.Any(x.SequenceEqual)).Should().BeTrue();
            map.CloseResultBox();
        }

        [AfterScenario]
        public void CloseApplication()
        {
            map.Close();
        }
    }
}
