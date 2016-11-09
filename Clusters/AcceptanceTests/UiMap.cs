using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;

namespace AcceptanceTests
{
    class UiMap : IDisposable
    {
        private bool closed;
        private readonly Application app;
        private readonly Window mainWindow;

        public UiMap(string path)
        {
            app = Application.Launch(path);
            mainWindow = app.GetWindow("Кластеризатор 3.0");
            closed = false;
        }

        public void Close()
        {
            app.Close();
            closed = true;
        }

        public void EnterPX(double x)
        {
            var px = mainWindow.Get<TextBox>("PX");
            px.BulkText = x.ToString(CultureInfo.InvariantCulture);
        }

        public void EnterPY(double y)
        {
            var py = mainWindow.Get<TextBox>("PY");
            py.BulkText = y.ToString(CultureInfo.InvariantCulture);
        }

        public void EnterP(double p)
        {
            var mvp = mainWindow.Get<TextBox>("MPVBox");
            mvp.BulkText = p.ToString(CultureInfo.InvariantCulture);
        }

        public void EnterEps(double eps)
        {
            var epsBox = mainWindow.Get<TextBox>("EPSBox");
            epsBox.BulkText = eps.ToString(CultureInfo.InvariantCulture);
        }

        public void EnterMinPoints(int min)
        {
            var minBox = mainWindow.Get<TextBox>("MP");
            minBox.BulkText = min.ToString(CultureInfo.InvariantCulture);
        }

        public void ChooseLpMetric()
        {
            var box = mainWindow.Get<ComboBox>("MSBox");
            box.Select("p-метрика");
        }

        public void ChooseSupMetric()
        {
            var box = mainWindow.Get<ComboBox>("MSBox");
            box.Select("супремум-метрика");
        }

        public void PressAddPoint()
        {
            var btn = mainWindow.Get<Button>("AddPointButton");
            btn.Click();
        }

        public void PressClusterize()
        {
            var btn = mainWindow.Get<Button>("ClusterizeButton");
            btn.Click();
        }

        public void PressResult()
        {
            var btn = mainWindow.Get<Button>("resultBtn");
            btn.Click();
        }

        public void PressDb()
        {
            var btn = mainWindow.Get<Button>("DbButton");
            btn.Click();
        }

        public Tuple<List<List<Tuple<double, double>>>, List<Tuple<double, double>>> GetClustersAndNoise()
        {
            var clusters = new List<List<Tuple<double, double>>>();
            

            string text = mainWindow.MessageBox("Убер кластеры").Get<Label>(SearchCriteria.Indexed(0)).Text;

            Regex clusterRegex = new Regex(@"Кластер \d+: {(.*)}");
            foreach (Match r in clusterRegex.Matches(text))
            {
                string line = r.Groups[1].Value;
                clusters.Add(Utility.GetListOfPoints(line));
            }

            Regex noiseRegex = new Regex(@"Шум: (.*)");
            var noise = Utility.GetListOfPoints(noiseRegex.Matches(text)[0].Groups[1].Value);

            return new Tuple<List<List<Tuple<double, double>>>, List<Tuple<double, double>>>(clusters, noise);
        }

        public void CloseResultBox()
        {
            mainWindow.MessageBox("Убер кластеры").Get<Button>(SearchCriteria.Indexed(0)).Click();
        }

        public void WaitWhileBusy()
        {
            app.WaitWhileBusy();
        }

        public void Dispose()
        {
            if (!closed)
            {
                app.Close();
            }
        }
    }
}
