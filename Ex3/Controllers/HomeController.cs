using Ex3.Models;
using System;
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
            return toXML(lon,lat);
        }

        
        
        [HttpGet]
        public ActionResult display(string ip, int port, int time)
        {
            client.Open(ip, port);
            ViewBag.time = time;
            return View();
        }

        [HttpPost]
        public string GetCoordinate()
        {
            string lon = client["get /position/longitude-deg\r\n"];
            string lat = client["get /position/latitude-deg\r\n"];
            return toXML(lon, lat);
        }

        public string toXML(string lon, string lat)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Point");

            writer.WriteElementString ("Lon", lon);
            writer.WriteElementString("Lat", lat);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        [HttpGet]
        public ActionResult displayFlight(string fileName, int time)
        {
            ViewBag.time = time;
            return View();
        }

        [HttpPost]
        public string GetState()
        {
            Info flightInfo = Info.Instance;
            
            return "temp";
        }
        

    }
}