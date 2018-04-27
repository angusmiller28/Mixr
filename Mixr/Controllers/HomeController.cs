using Mixr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mixr.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        Entities db = new Entities();

        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetSearchingData(string SearchBy, string SearchValue)
        {
            List<Product> productList = new List<Product>();
            if (SearchBy == "ID")
            {
                try
                {
                    int Id = Convert.ToInt32(SearchValue);
                    productList = db.Products.Where(x => x.Id == Id || SearchValue == null).ToList();
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} is not a ID ", SearchValue);
                }
                return Json(productList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                productList = db.Products.Where(x => x.Name.Contains(SearchValue) || SearchValue == null).ToList();
                JsonResult data = Json(productList, JsonRequestBehavior.AllowGet);

                return Json(productList, JsonRequestBehavior.AllowGet);
            }

        }
    }
}