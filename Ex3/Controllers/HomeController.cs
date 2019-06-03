using Ex3.Models;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Text;
using System.IO;

namespace Ex3.Controllers
{
    public class HomeController : Controller
    {
        private Connect client = Connect.Instance;
        private TimeTracker timer = TimeTracker.Instance;
        // GET: Home

        /**
         * the function return the default view
         */
        public ActionResult Index()
        {
            return View();
        }

        /**
         * params: ip, port, time interval
         * the function open communication with the givin ip and port
         * and save the time and call display view
         */
        [HttpGet]
        public ActionResult displayFromServer(string ip, int port, int time)
        {
            client.Open(ip, port);
            ViewBag.time = time;
            return View("display");
        }

        /**
         * params: ip, port, rate - time interval, time - run duration, fileName
         * the function open communication with the givin ip and port
         * and save to the viewBag the rate and to session the file.
         * and reutrn the view.
         */
        [HttpGet]
        public ActionResult save(string ip, int port, int rate, int time, string fileName)
        {
            client.Open(ip, port);
            ViewBag.time = rate;
            timer.Interval = time;
            Info.Instance.OpenFile(fileName);
            return View();
        }

        /**
         * returnVal - string builder
         * the function init timer, and get info of lon,lat, throttle and rudder
         * from the client
         * if the timer not done save to file
         * if done rerurn the lon and lat val
         */
        [HttpPost]
        public string RouteSaver()
        {
            timer.StartTimer();
            Info flightInfo = Info.Instance;
            string lon = client["get /position/longitude-deg\r\n"];
            string lat = client["get /position/latitude-deg\r\n"];
            if (timer.isRunning)
            {
                string throttle = client["get /controls/engines/current-engine/throttle\r\n"];
                string rudder = client["get /controls/flight/rudder\r\n"];
                flightInfo.WriteToFile(lon,lat,throttle,rudder);
            }
            List<string> param = new List<string>();
            param.Add(lon);
            param.Add(lat);
            return toXML(param);
        }


        /**
         * param: ip, port, time.
         * the function get ip port and time
         * if the string is valid ip c- call displayFromServer
         * else call displayFlight
         */
        [HttpGet]
        public ActionResult display(string ip, int port, int time)
        {
            try
            {
                IPAddress.Parse(ip);
                return displayFromServer(ip, port, time);
            }
            catch (FormatException e)
            {
                return displayFlight(ip, port);
            }
        }

        /**
         * returnVal - string builder.
         * the function gets from the client the lon and lat val
         * and save in XML
         */
        [HttpPost]
        public string GetCoordinate()
        {
            List<string> param = new List<string>();
            param.Add(client["get /position/longitude-deg\r\n"]);
            param.Add(client["get /position/latitude-deg\r\n"]);
            return toXML(param);
        }

        /**
         * param: the function gets list of string - parameters
         * create new XML file
         * save the parameters to the XML
         * and return the string builder.
         */
        public string toXML(List<string> param)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            
            writer.WriteStartDocument();
            if (param.Count > 1)
            {
                writer.WriteStartElement("Point");

                writer.WriteElementString("Lon", param[0]);
                writer.WriteElementString("Lat", param[1]);
            }
            else
            {
                writer.WriteStartElement("alert");
                writer.WriteElementString(param[0],param[0]);
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            return sb.ToString();
        }

        /**
         * params: fileName, time
         * the function read data from the file
         * save the time
         * and call the displayFlight view.
         */
        [HttpGet]
        public ActionResult displayFlight(string fileName, int time)
        {
            Info.Instance.ReadData(fileName);
            ViewBag.time = time;
            return View("displayFlight");
        }

        /**
         * returnVal: string builder
         * the function get the next line of data
         * split it
         * and send to XML
         */
        [HttpPost]
        public string GetState()
        {
            string input = Info.Instance.Next;
            List<string> param = new List<string>();
            if (input == "") param.Add("error");
            else if (input == "END") param.Add("END");
            else
            {
                param = input.Split(',').ToList();
            }
            return toXML(param);
        }
    }
}