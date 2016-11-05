using System;
using TechTalk.SpecFlow;

namespace AcceptanceTests
{
    [Binding]
    public class ClusterizationSteps
    {
        [Given(@"I entered points \{\((.*)\),\((.*)\),\((.*);(.*)\),\((.*);(.*)\),\((.*)\),\((.*);(.*)\),\((.*);(.*)\), \((.*)\); \((.*)\)}")]
        public void GivenIEnteredPoints(string p0, string p1, Decimal p2, Decimal p3, int p4, Decimal p5, string p6, Decimal p7, Decimal p8, Decimal p9, int p10, string p11, string p12)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I entered minimal number of points (.*)")]
        public void GivenIEnteredMinimalNumberOfPoints(int p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I entered epsilon (.*)")]
        public void GivenIEnteredEpsilon(int p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I chose metric LP\((.*)\)")]
        public void GivenIChoseMetricLP(int p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I pressed clusterize")]
        public void WhenIPressedClusterize()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"There must be clusters \{\((.*)\),\((.*)\),\((.*);(.*)\),\((.*);(.*)\)}, \{\((.*)\),\((.*);(.*)\),\((.*);(.*)\)} and noise \{\((.*)\); \((.*)\)}")]
        public void ThenThereMustBeClustersAndNoise(string p0, string p1, Decimal p2, Decimal p3, int p4, Decimal p5, string p6, Decimal p7, Decimal p8, Decimal p9, int p10, string p11, string p12)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
