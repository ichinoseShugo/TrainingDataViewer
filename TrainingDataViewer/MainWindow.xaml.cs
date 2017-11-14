using System;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TrainingDataViewer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public string directoryPath = "";
        string[] imageFilePathes;
        int[] imageFileTimes;
        int imageIndex;
        BitmapImage bitmap = null;

        DataList dataList;
        List<double[]> rowList = new List<double[]>();

        PlotViewModel model;
        List<string> selectedNames = new List<string>(); 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenDirectoryDialog();
            GetImages();
            InitializeView();
        }

        private void InitializeView()
        {
            //jpg表示関連
            ShowImage(imageFilePathes[0]);//画像表示
            ImageLabel.Content = imageFilePathes[0];//画像ラベル表示

            //グラフ関連
            dataList = new DataList(directoryPath);
            this.NameBox.ItemsSource = dataList.DataNames;

            model = new PlotViewModel(dataList);
            MyPlot.Model = model.GetModel();
        }

        private void DirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            OpenDirectoryDialog();
        }

        private void OpenDirectoryDialog()
        {
            var dialog = new CommonOpenFileDialog("保存フォルダ選択");
            // フォルダ選択モード。
            dialog.IsFolderPicker = true;
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Kinect\TrainingData";
            
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.DirectoryPathBox.Content = dialog.FileName;
                directoryPath = dialog.FileName;
            }
        }

        private void GetImages()
        {
            imageFilePathes = Directory.GetFiles(
               directoryPath + "\\image", "*.jpg", SearchOption.AllDirectories);

            string[] samples = imageFilePathes[0].Split('\\');
            if (samples[samples.Length - 1].Length < 12) RenameImage();

            imageFileTimes = new int[imageFilePathes.Length];
            for (int i = 0; i < imageFilePathes.Length; i++)
            {
                string[] token = imageFilePathes[i].Split('\\');//fileはパス名なので\で分割
                imageFileTimes[i] = int.Parse(NumMatcher(token[token.Length - 1]));
            }

            Slider.Minimum = imageFileTimes[0];
            DataLabel.Content = imageFileTimes[0];
            Slider.Maximum = imageFileTimes[imageFileTimes.Length-1];
        }

        private string NumMatcher(string target)
        {
            //Regexオブジェクトを作成
            Regex r = new Regex(@"[1-9][0-9]+", RegexOptions.IgnoreCase);

            //TextBox1.Text内で正規表現と一致する対象を1つ検索
            Match m = r.Match(target);

            string match = "";
            while (m.Success)
            {
                //一致した対象が見つかったときキャプチャした部分文字列を表示
                match = m.Value; 
                //次に一致する対象を検索
                m = m.NextMatch();
            }
            return match;
        }

        /// <summary>
        /// jpgの名前を変える
        /// </summary>
        private void RenameImage()
        {
            foreach (var file in imageFilePathes)
            {
                //名前変更の準備
                string[] token = file.Split('\\');//fileはパス名なので\で分割
                string last = token[token.Length - 1];//jpgファイル名

                while (last.Length < 12) last = "0" + last;//0を追加

                //名前変更後ファイルのパスの作成
                string renamePath = "C:";
                for (int i = 1; i < token.Length - 1; i++) renamePath += "\\" + token[i];
                renamePath += "\\" + last;

                //名前変更
                File.Move(@file, @renamePath);
            }

            //再びファイルパス取得
            GetImages();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            imageIndex = CloserImageIndex(e.NewValue);
            if (imageFilePathes != null)
            {
                ShowImage(imageFilePathes[imageIndex]);
                ImageLabel.Content = imageFilePathes[imageIndex];
            }
            if (model != null)
            {
                model.ChangePositionX(imageFileTimes[imageIndex]);
                MyPlot.Model.InvalidatePlot(true);
                DataLabel.Content = e.NewValue;
            }
        }

        private int CloserImageIndex(double value)
        {
            double dist = 999999999999999999;
            for (int i=0; i<imageFileTimes.Length; i++)
            {
                double diff = Math.Abs(imageFileTimes[i] - value);
                if (dist < diff)
                {
                    return i-1;
                }
                else
                {
                    dist = diff;
                }
            }
            return imageFileTimes.Length-1;
        }

        private void ShowImage(string filename)
        {
            // 既に読み込まれていたら解放する
            if (bitmap != null)
            {
                bitmap = null;
            }
            // BitmapImageにファイルから画像を読み込む
            bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filename);
            bitmap.EndInit();
            // Imageコントロールに表示
            Image.Source = bitmap;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            NameBox.SelectedIndex = -1;
        }

        private void NameBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            model.ClearSeries();
            selectedNames.Clear();
            if (NameBox.SelectedItems.Count > 0)
            {
                foreach (var name in NameBox.SelectedItems)
                {
                    selectedNames.Add(name.ToString());
                    model.AddLineSeries(name.ToString());
                }
                if (PositionVisibility.IsChecked == true) model.AddLineSeries("ImagePosition");
            }
            model.SetSlectedSeries(selectedNames);
            model.ChangePositionX(imageFileTimes[imageIndex]);
            MyPlot.Model.InvalidatePlot(true);
        }

        private void PositionVisibility_Click(object sender, RoutedEventArgs e)
        {
            if(PositionVisibility.IsChecked == true && NameBox.SelectedItems.Count > 0)
            {
                model.AddLineSeries("ImagePosition");
            }
            if (PositionVisibility.IsChecked == false)
            {
                model.RemoveSeries("ImagePosition");
            }
            MyPlot.Model.InvalidatePlot(true);
        }
    }
}