﻿using System;
using System.Collections;
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
        private List<string> fromFile = null;
        private IEnumerator reader = null;
        
        /**
         * singleton
         */
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

        /**
         * the function get the next line from the iterator and return it
         * if there is no file - return ""
         * if the iter done - return END
         */
        public string Next
        {
            get
            {
                if (fromFile == null) return "";
                if(reader.MoveNext())
                {
                    return Convert.ToString(reader.Current);
                } else
                {
                    return "END";
                }
            }
        }

        /**
         * param :string fileName, string lon, string lat, string throttle, string rudder
         * the fnction open file and save the parameter
         */
        public void WriteToFile(string fileName, string lon, string lat,
            string throttle, string rudder)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
            {
                file.WriteLine(lon + "," + lat + "," + throttle + "," + rudder);
               
            }
        }

        /**
         * param: fileName
         * the function get file name
         * open and read from the file to list.
         */
        public void ReadData(string filename)
        {
            fromFile = new List<string>();
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, filename));
            using (System.IO.StreamReader fileReader = new System.IO.StreamReader(path, true))
            {
                string line;
                while ((line = fileReader.ReadLine()) != null)
                {
                    fromFile.Add(line);
                }
            }
            reader = fromFile.GetEnumerator();
        }
    }
       
}