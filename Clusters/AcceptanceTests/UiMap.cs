using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.UIItems;
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

        public void PressDb()
        {
            var btn = mainWindow.Get<Button>("DbButton");
            btn.Click();
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
