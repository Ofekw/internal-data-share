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
            //Set the home page to be the compiled react page
            ViewBag.Title = "Home Page";
            string path = Server.MapPath("~");
            path = Path.GetFullPath("D:/home/site/repository/frontend/");      //(Path.Combine(path, @"..\..\..\Frontend\"));
            return File(path + "index.html", "text/html");
        }
    }
}
