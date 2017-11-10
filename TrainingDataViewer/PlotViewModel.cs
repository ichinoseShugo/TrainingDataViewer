using System;
using System.Collections;
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
        public PlotModel MyPlotModel;
        /// <summary>
        /// 読み込んだファイル内の各列についている名前の配列
        /// </summary>
        public string[] DataNames;
        /// <summary>
        /// 名前をkeyとしてvalueをSeriesとしたHash
        /// </summary>
        private Hashtable NameToSeries = new Hashtable();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataList"></param>
        public PlotViewModel(DataList dataList)
        {
            this.MyPlotModel = new PlotModel();
            this.DataNames = dataList.DataNames;
            foreach (var dataname in this.DataNames)
            {
                CreateLineSeries(dataname, 1, dataList.GetDataList(dataname));
            }
            CreateLineSeries("ImagePosition", 2, new List<double[]>(){ new double[]{0, 0}, new double[] { 0, 10 }, });

            //this.MyPlotModel.Series.Add((LineSeries)NameToSeries["ImagePosition"]);
        }

        /// <summary>
        /// DataNameとDataListからSeriesのを作成しDataNameと紐付ける
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="tickness"></param>
        /// <param name="dataList"></param>
        public void CreateLineSeries(String dataName, int tickness, List<double[]> dataList)
        {
            IList<DataPoint> points = new List<DataPoint>();
            foreach (var list in dataList)
            {
                points.Add(new DataPoint(list[0], list[1]));
            }
            LineSeries lineSeries = new LineSeries
            {
                Title = dataName,
                StrokeThickness = tickness,
                ItemsSource = points,
            };
            NameToSeries.Add(dataName, lineSeries);
        }

        /// <summary>
        /// スライダーの値からPositionSeriesの位置を変更
        /// </summary>
        /// <param name="value"></param>
        public void ChangePositionSeries(double sliderValue)
        {
            ((LineSeries)NameToSeries["ImagePosition"]).ItemsSource = new List<DataPoint>
            {
                new DataPoint(sliderValue, 0),
                new DataPoint(sliderValue, 10)
            };
        }
        
        /// <summary>
        /// 描画するSeriesを追加
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="dataList"></param>
        public void AddLineSeries(String dataName)
        {
            this.MyPlotModel.Series.Add((LineSeries)NameToSeries[dataName]);
        }
        
        /// <summary>
        /// position以外のSeriesをクリア
        /// </summary>
        public void ClearSeries()
        {
            if(this.MyPlotModel.Series.Count>0) this.MyPlotModel.Series.Clear();
        }

        /// <summary>
        /// DataNameのSeriesが含まれているかを返す
        /// </summary>
        /// <param name="dataName"></param>
        /// <returns></returns>
        public bool Contains(string dataName)
        {
            return this.MyPlotModel.Series.Contains((LineSeries)NameToSeries[dataName]);
        }

        public PlotModel GetModel()
        {
            return MyPlotModel;
        }
    }
}
