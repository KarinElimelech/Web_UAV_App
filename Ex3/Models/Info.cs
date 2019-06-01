using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class Info
    {
        private static Info instance = null;
        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";

        public static Info Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Info();
                }
                return instance;
            }
        }

        public void WriteToFile(string fileName, string lon, string lat,
            string throttle, string rudder)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            //FileStream stream = File.Open(path,FileMode.Open, FileAccess.Write);   
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
            {
                file.WriteLine(lon + "," + lat + "," + throttle + "," + rudder);
            }
        }

        //TODO - change to useful .. 
        public string[] ReadData(string filename)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, filename));
            return System.IO.File.ReadAllLines(path);
             
        }
    }
       
}