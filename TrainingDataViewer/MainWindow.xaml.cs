using System;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace TrainingDataViewer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public string directoryPath = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //OpenDirectoryDialog();
            //GetImages();
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
            dialog.InitialDirectory = @"C:\Users\S\Documents\Kinect\TrainingData";
            
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.directoryPathBox.Text = dialog.FileName;
                directoryPath = dialog.FileName;
            }
        }

        private void GetImages()
        {
            string[] files = System.IO.Directory.GetFiles(
               directoryPath + "\\image", "*.bmp", System.IO.SearchOption.AllDirectories);
            foreach (string file in files)
            {
                Console.WriteLine(file);
            }
        }
    }
}