using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coevolution.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            //return View();
            string path = Server.MapPath("~");
            path = Path.GetFullPath(Path.Combine(path, @"..\..\..\Frontend\"));

            return File(path + "index.html", "text/html");
        }
    }
}
