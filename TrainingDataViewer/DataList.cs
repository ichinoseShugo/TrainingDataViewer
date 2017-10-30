using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingDataViewer
{
    class DataList
    {
        public static List<ArrayList> dataList = new List<ArrayList>();

        public DataList(string directoryPath)
        {
            string[] files = Directory.GetFiles(directoryPath, "*.csv", System.IO.SearchOption.AllDirectories);
            foreach (var file in files)
            {
                Console.WriteLine(file);
                using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
                {
                    string line = "";
                    ArrayList list = new ArrayList();

                    // test.txtを1行ずつ読み込んでいき、末端(何もない行)までwhile文で繰り返す
                    while ((line = sr.ReadLine()) != null)
                    {
                        String[] token = line.Split(',');
                        list.Add(token);
                    }
                    dataList.Add(list);
                }
            }
        }
    }
}