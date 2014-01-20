using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MortgageCalculator.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Mortgage/

        public ActionResult Index()
        {
            return View();
        }

    }
}
