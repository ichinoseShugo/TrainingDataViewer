using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OxyPlot;
using OxyPlot.Series;

namespace TrainingDataViewer
{
    class PlotViewModel
    {
        public PlotModel MyModel;
        
        public PlotViewModel()
        {
            this.MyModel = new PlotModel();
        }

        public void ChangePositionSeries(double value)
        {
            //this.MyModel.Series[0]
            //this.MyModel.Series.RemoveAt(this.MyModel.Series.Count - 1);
            //this.MyModel.Series.Remove();
            IList<DataPoint> position = new List<DataPoint>
            {
                new DataPoint(value, 0),
                new DataPoint(value, 10)
            };
            LineSeries lineSeries = new LineSeries
            {
                ItemsSource = position,
                StrokeThickness = 2,
                Title = "position",
            };
            this.MyModel.Series.Add(lineSeries);
        }

        public void AddLineSeries(String dataName, List<double[]> dataList)
        {
            IList<DataPoint> points = new List<DataPoint>();
            foreach(var list in dataList) points.Add(new DataPoint(list[0], list[1]));
            LineSeries lineSeries = new LineSeries
            {
                ItemsSource = points,
                StrokeThickness = 1,
                Title = dataName,
            };
            this.MyModel.Series.Add(lineSeries);
            //this.MyModel.Series.Insert(this.MyModel.Series.Count-1,lineSeries);
        }
        
        public void ClearSeries()
        {
            if(this.MyModel.Series.Count>0) this.MyModel.Series.Clear();
        }

        public void RemoveSeries(string dataname)
        {
            //;
            //if (this.MyModel.Series.Count > 0) this.MyModel.Series.Remove();
        }

        public PlotModel GetModel()
        {
            return MyModel;
        }
    }
}
