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
        public string[] DataNames;

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
                    DataNames = line.Split(',');//データの名前

                    //「double配列を要素に持つリスト」の配列を用意
                    List<double[]>[] data = new List<double[]>[DataNames.Length - 1];
                    for (int i = 0; i < data.Length; i++) data[i] = new List<double[]>();
                    //foreach (var d in DataNames) Console.WriteLine(d);

                    //double配列を準備（第1,2インデックスにそれぞれ時間，データが入る）
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine("line:"+line);
                        String[] token = line.Split(',');//token[0]=time， token[n]=データ(n>0)
                        if (token.Length == DataNames.Length)//tokenの長さが列の数と等しくない場合，そのlineは遅延時間が記述してある
                        {
                            //timeを記述してある列以外の全列のdouble配列を用意する
                            for (int j = 1; j < token.Length; j++)//token[0]はtimeの値なのでj=1から始める
                            {
                                double[] TimeAndData = new double[2];
                                TimeAndData[0] = Double.Parse(token[0]);//token[0](time)をdouble配列の初めのインデックスに
                                TimeAndData[1] = Double.Parse(token[j]);//token[1](data)を次のインデックスに
                                data[j-1].Add(TimeAndData);//リストに配列を追加(dataリストの数はDataNamesより1低い)
                            }
                        }
                    }
                    for (int k = 1; k < data.Length; k++) NameToData.Add(DataNames[k], data[k-1]);//名前とデータ配列の対応付け

                    foreach (var d in data)
                    {
                        //Console.WriteLine(d);
                        foreach (var st in d)
                        {
                            //Console.WriteLine(st);
                            //Console.WriteLine(st[0] + ":" + st[1]);
                        }
                    }
                }
            }
        }

        public List<double> getDataList(string DataName)
        {
            return (List<double>)NameToData[DataNames];
        }
    }
}