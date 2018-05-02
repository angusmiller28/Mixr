using Mixr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mixr.Controllers
{
    public class CategoryController : Controller
    {
        Entities db = new Entities();
        // GET: Category
        public ActionResult Index()
        {
            var categories = db.Categories.ToList();
            return View(categories);
        }

        // GET: /Category/Manage
        public ActionResult Manage()
        {
            // prepopulat roles for the view dropdown
            var list = db.Categories.OrderBy(r => r.Name).ToList().Select(rr =>
                new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Categories = list;
            return View();
        }

        // GET: /Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                db.Categories.Add(new Category
                {
                    Name = collection["CategoryName"]
                });
                db.SaveChanges();
                ViewBag.ResultMessage = "Category created successfully !";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: /Category/DELETE
        public ActionResult Delete(string categoryName)
        {
            var thisCategory = db.Categories.Where(r => r.Name.Equals(categoryName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            db.Categories.Remove(thisCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //
        // GET: /Category/Edit/5
        public ActionResult Edit(string categoryName)
        {
            var thisCategory = db.Categories.Where(r => r.Name.Equals(categoryName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisCategory);
        }

        //
        // POST: /Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            try
            {
                db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

