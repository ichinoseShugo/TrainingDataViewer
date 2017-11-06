using System;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Media.Imaging;
using System.IO;

namespace TrainingDataViewer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public string directoryPath = "";
        string[] imageFiles;
        BitmapImage bitmap = null;

        DataList dataList;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenDirectoryDialog();
            GetImages();
            RenameImage();
            ShowImage(imageFiles[0]);
            InitializeView();
        }

        private void InitializeView()
        {
            dataList = new DataList(directoryPath);
            DataNamesBox.ItemsSource = dataList.DataNames;
            foreach(var names in dataList.DataNames)
            {

            }
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
            imageFiles = Directory.GetFiles(
               directoryPath + "\\image", "*.jpg", SearchOption.AllDirectories);
            if (imageFiles.Length > 0) Slider.Maximum = imageFiles.Length;
            //foreach(var file in imageFiles)Console.WriteLine(file);
        }

        /// <summary>
        /// jpgの名前を変える
        /// </summary>
        private void RenameImage()
        {
            //jpgの名前の長さが12なら必要なし
            string[] samples = imageFiles[0].Split('\\');
            if (samples[samples.Length - 1].Length == 12)
            {
                //MessageBox.Show("FileName is OK");
                return;
            }

            //rename作業
            foreach (var file in imageFiles)
            {
                //名前変更の準備
                string[] token = file.Split('\\');//fileはパス名なので\で分割
                string last = token[token.Length - 1];//jpgファイル名
                while (last.Length < 12) last = "0" + last;//0を追加
                
                //名前変更後ファイルのパスの作成
                string renamePath = "C:";
                for (int i = 1; i < token.Length - 1; i++) renamePath +="\\" + token[i];
                renamePath +="\\" + last;
                
                //名前変更
                File.Move(@file, @renamePath);
            }

            //再びファイルパス取得
            GetImages();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (imageFiles != null)
            {
                ShowImage(imageFiles[(int)(e.NewValue - 1)]);
            }
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

        private void DataNamesBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }
    }
}