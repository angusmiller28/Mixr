using Mixr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mixr.Controllers
{
    public class SearchController : Controller
    {

        Entities db = new Entities();

        // GET: Search
        public PartialViewResult Index(string searching)
        {
            ProductDTO product = new ProductDTO();
            Category category = new Category();

            var model = new SearchViewModel()
            {
                ProductsCollection = product.GetProducts(),
                CategoriesCollection = db.Categories.ToList(),
            };

            return PartialView(model);
        }

        public JsonResult GetSearchingData(string SearchBy, string SearchValue)
        {
            ProductDTO product = new ProductDTO();
            IEnumerable<ProductDTO> StuList = product.GetProducts();
            if (SearchBy == "ID")
            {
                try
                {
                    int Id = Convert.ToInt32(SearchValue);
                    StuList = product.GetProduct(Id);
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} is Not a ID", SearchValue);
                }
                return Json(StuList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                StuList = product.GetProductByName(SearchValue);
                return Json(StuList, JsonRequestBehavior.AllowGet);
            }

        }
    }
        
}