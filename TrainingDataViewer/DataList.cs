using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingDataViewer
{
    class DataList
    {
        public static List<List<double>> List = new List<List<double>>();
        public static List<ArrayList> d = new List<ArrayList>();

        public DataList(string directoryPath)
        {
            string[] files = System.IO.Directory.GetFiles(directoryPath, "*.csv", System.IO.SearchOption.AllDirectories);
            foreach(var s in files)Console.WriteLine(s);
            ArrayList dd = new ArrayList();
            dd.Add("d");
            dd.Add(1);
            dd.Add("d");
            dd.Add(1);
        }
    }
}
