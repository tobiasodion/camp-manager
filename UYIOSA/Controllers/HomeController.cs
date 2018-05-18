using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UYIOSA.Models;

namespace UYIOSA.Controllers
{
    public class HomeController : Controller
    {
        uyiosadb db = new Models.uyiosadb();
        // GET: Home
        public ActionResult Index()
        {
            var model = db.Admin.ToList();
            return View(model);
        }
    }
}