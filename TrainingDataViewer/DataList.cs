using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingDataViewer
{
    public class DataList
    {
        /// <summary>
        /// 名前をkeyとしてvalueをdouble配列としたHash
        /// </summary>
        private Hashtable NameToData = new Hashtable();
        /// <summary>
        /// 読み込んだファイル内の各列についている名前の配列
        /// </summary>
        public string[] DataNames = new string[0];

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="directoryPath"></param>
        public DataList(string directoryPath)
        {
            //ディレクトリ以下の全てのcsvファイルのパスを取得
            string[] files = Directory.GetFiles(directoryPath, "*.csv", SearchOption.AllDirectories);

            //ファイルからデータリストを作成
            FileToDataList(files);
        }

        /// <summary>
        /// ファイルからデータリストを作成
        /// </summary>
        /// <param name="files"></param>
        private void FileToDataList(string[] files)
        {
            foreach (var file in files)
            {
                using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
                {
                    string line = sr.ReadLine();//ファイルのヘッダを読み込む
                    string[] RowNames = line.Split(',');//列の名前

                    // "double配列を要素に持つリスト(このリスト一つで列となる)" の配列を用意
                    List<double[]>[] data = new List<double[]>[RowNames.Length];
                    for (int i = 0; i < data.Length; i++) data[i] = new List<double[]>();

                    //double配列を準備（第1,2インデックスにそれぞれX軸データ，Y軸データが入る）
                    int lineCount = 0;//time列のX軸よう行数カウント
                    while ((line = sr.ReadLine()) != null)
                    {
                        String[] token = line.Split(',');//token[0]=time， token[n]=データ(n>0)
                        //tokenの長さが列の数と等しくない場合，そのlineは遅延時間が記述してある
                        if (token.Length == RowNames.Length)
                        {
                            //time列のdouble配列をリストに追加する
                            double[] TimeAndData = new double[2];
                            TimeAndData[0] = lineCount++;//X軸は1刻み
                            TimeAndData[1] = Double.Parse(token[0]);//Y軸にtime
                            data[0].Add((double[])TimeAndData.Clone());//列リストの0にtime列を追加

                            //time以外の列のdouble配列をリストに追加
                            for (int j = 1; j < token.Length; j++)//token[0]はtimeの値なのでj=1から始める
                            {
                                TimeAndData[0] = Double.Parse(token[0]);//X軸はtoken[0](time)
                                TimeAndData[1] = Double.Parse(token[j]);//Y軸にその列のセルデータ
                                data[j].Add((double[])TimeAndData.Clone());//double配列を列リストに追加
                            }
                        }
                    }

                    //hashによる名前(key)とデータ配列(value)の対応付け
                    for (int k = 0; k < data.Length; k++) NameToData.Add(RowNames[k], data[k]);

                    //DataNames配列に列の名前を追加する
                    string[] mergedArray = new string[DataNames.Length + RowNames.Length];
                    //マージする配列のデータをコピーする
                    Array.Copy(DataNames, mergedArray, DataNames.Length);
                    Array.Copy(RowNames, 0, mergedArray, DataNames.Length, RowNames.Length);
                    DataNames = mergedArray;
                }
            }
        }

        /// <summary>
        /// データの名前からdouble配列リスト([0]:X軸 [1]:Y軸)を取得
        /// </summary>
        /// <param name="DataName"></param>
        /// <returns></returns>
        public List<double[]> GetDataList(string DataName)
        {
            return (List<double[]>)NameToData[DataName];
        }
    }
}