using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OxyPlot;
using OxyPlot.Wpf;
using OxyPlot.Series;

namespace TrainingDataViewer
{
    class PlotViewModel
    {
        public PlotModel MyModel { get; private set; }
        //public List<> series = new List<>();

        public PlotViewModel()
        {
            this.MyModel = new PlotModel();

            var lineSeries = new OxyPlot.Series.LineSeries();
            IList<DataPoint> points = new List<DataPoint>
            {
                new DataPoint(1,0),
                new DataPoint(1,1)
            };
            lineSeries.ItemsSource = points;
            this.MyModel.Series.Add(lineSeries);
        }

        public void AddLineSeries(List<double[]> dataList)
        {
            var lineSeries = new OxyPlot.Series.LineSeries();
            IList<DataPoint> points = new List<DataPoint>();
            foreach(var list in dataList) points.Add(new DataPoint(list[0], list[1]));
            lineSeries.ItemsSource = points;
            this.MyModel.Series.Add(lineSeries);
        }

        public void AddBarSeries(List<double[]> dataList)
        {
            var BarSeries = new OxyPlot.Series.BarSeries();
            IList<DataPoint> points = new List<DataPoint>();
            foreach (var list in dataList) points.Add(new DataPoint(list[0], list[1]));
            BarSeries.ItemsSource = points;
            this.MyModel.Series.Add(BarSeries);
        }

        public void ClearSeries()
        {
            this.MyModel.Series.Clear();
        }

        public void RemoveSeries()
        {

        }

        public PlotModel GetModel()
        {
            return MyModel;
        }
    }
}
