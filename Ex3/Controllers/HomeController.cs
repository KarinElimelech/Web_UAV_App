using Ex3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Text;

namespace Ex3.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult display(string ip, int port, int time)
        {
            Connect.Instance.Open(ip, port);
            //Session["time"] = time;
            ViewBag.time = time;
            return View();
        }

        [HttpPost]
        public string GetCoordinate()
        {
            return toXML(Connect.Instance.Lon, Connect.Instance.Lat);
        } 

        public string toXML(double lon, double lat)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Point");

            writer.WriteElementString ("Lon", lon.ToString());
            writer.WriteElementString("Lat", lat.ToString());

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        } 
    }
}