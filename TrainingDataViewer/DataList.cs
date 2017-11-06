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
        public Hashtable NameToData = new Hashtable();
        public string[] DataNames = new string[0];

        public DataList(string directoryPath)
        {
            string[] files = Directory.GetFiles(directoryPath, "*.csv", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                Console.WriteLine(file);
                using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
                {
                    string line = sr.ReadLine();
                    //Console.WriteLine(line);
                    string[] RowNames = line.Split(',');//列の名前

                    // "double配列を要素に持つリスト(このリスト一つで列となる)" の配列を用意
                    List<double[]>[] data = new List<double[]>[RowNames.Length];
                    for (int i = 0; i < data.Length; i++) data[i] = new List<double[]>();

                    //double配列を準備（第1,2インデックスにそれぞれX軸データ，Y軸データが入る）
                    int lineCount = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine("line:"+line);
                        String[] token = line.Split(',');//token[0]=time， token[n]=データ(n>0)
                        if (token.Length == RowNames.Length)//tokenの長さが列の数と等しくない場合，そのlineは遅延時間が記述してある
                        {
                            //Console.WriteLine(lineCount + ":" + token[0]);
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
                    //foreach (var name in RowNames) Console.WriteLine(name);
                    for (int k = 0; k < data.Length; k++)
                    {
                        //Console.WriteLine(RowNames[k]);
                        //Console.WriteLine(data[k]);
                        //foreach(var d in data[k]) Console.WriteLine(d[0] + ":" + d[1]);
                        NameToData.Add(RowNames[k], data[k]);//名前とデータ配列の対応付け
                    }
                    
                    //DataNames配列に列の名前を追加する
                    string[] mergedArray = new string[DataNames.Length + RowNames.Length];
                    //マージする配列のデータをコピーする
                    Array.Copy(DataNames, mergedArray, DataNames.Length);
                    Array.Copy(RowNames, 0, mergedArray, DataNames.Length, RowNames.Length);
                    DataNames = mergedArray;
                }
            }
        }

        public List<double[]> getDataList(string DataName)
        {
            return (List<double[]>)NameToData[DataName];
        }
    }
}