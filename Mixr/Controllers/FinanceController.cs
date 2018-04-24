using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mixr.Controllers
{
    public class FinanceController : Controller
    {
        // GET: Finance
        public ActionResult Index()
        {
            return View();
        }

        // GET: Finance/Purchases
        public ActionResult Purchases()
        {
            return View();
        }
    }
}