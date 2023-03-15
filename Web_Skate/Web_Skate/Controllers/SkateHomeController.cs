using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Skate.Controllers
{
    public class SkateHomeController : Controller
    {
        
        // GET: SkateHome
        public ActionResult Index()
        {
            return View();
        }
    }
}