using Mixr.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mixr.Controllers
{
    public class ProductController : Controller
    {
        Entities db = new Entities();

        // GET: Product
        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
        }

        // GET: /Product/Create
        public ActionResult Create()
        {
            // prepopulat roles for the view dropdown
            var list = db.Categories.OrderBy(r => r.Name).ToList().Select(rr =>
                new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Categories = list;

            return View();
        }

        // POST: /Product/DELETE
        public ActionResult Delete(int Id)
        {
            var thisProduct = db.Products.Where(x => x.Id == Id).FirstOrDefault();
            db.Products.Remove(thisProduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: /Product/Create
        [HttpPost]
        public ActionResult Create(Product model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // upload image to file and set image field to image filename
            model.Image = model.ProductImage.FileName;
            string path = Path.Combine(Server.MapPath("~/Uploads/Products"), Path.GetFileName(model.ProductImage.FileName));
            model.ProductImage.SaveAs(path);

            // insert product into database

            db.Products.Add(model);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: /Product/Edit/5
        public ActionResult Edit(int Id)
        {
            var thisProduct = db.Products.Where(r => r.Id == Id).FirstOrDefault();

            // prepopulat roles for the view dropdown
            var list = db.Categories.OrderBy(r => r.Name).ToList().Select(rr =>
                new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Roles = list;

            return View(thisProduct);
        }

        // POST: /Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            try
            {
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
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