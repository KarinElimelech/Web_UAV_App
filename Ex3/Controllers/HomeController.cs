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
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult displayFromServer(string ip, int port, int time)
        {
            client.Open(ip, port);
            ViewBag.time = time;
            return View("display");
        }

        [HttpGet]
        public ActionResult save(string ip, int port, int rate, int time, string fileName)
        {
            client.Open(ip, port);
            ViewBag.time = rate;
            timer.Interval = time;
            Session["file"] = fileName;
            return View();
        }


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
                flightInfo.WriteToFile(Convert.ToString(Session["file"]),lon,lat,throttle,rudder);
            }
            List<string> param = new List<string>();
            param.Add(lon);
            param.Add(lat);
            return toXML(param);
        }

        
        
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

        [HttpPost]
        public string GetCoordinate()
        {
            List<string> param = new List<string>();
            param.Add(client["get /position/longitude-deg\r\n"]);
            param.Add(client["get /position/latitude-deg\r\n"]);
            return toXML(param);
        }

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


        [HttpGet]
        public ActionResult displayFlight(string fileName, int time)
        {
            Info.Instance.ReadData(fileName);
            ViewBag.time = time;
            return View("displayFlight");
        }

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