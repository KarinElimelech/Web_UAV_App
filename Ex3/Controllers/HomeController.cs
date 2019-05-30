﻿using Ex3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult display(string ip, int port)
        {
            Connect.Instance.Open(ip,port);
            ViewBag.lon = Connect.Instance.Lon;
            ViewBag.lat = Connect.Instance.Lat;
            return View();
        }

        

    }
}