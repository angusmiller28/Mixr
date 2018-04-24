using System.Data;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomatedTellerMachine.Models;

namespace AutomatedTellerMachine.Controllers
{
    public class ArtistController : Controller
    {
        private MixrDBEntities _db = new MixrDBEntities();

        // GET: Artist
        public ActionResult Index()
        {
            return View(_db.Artists.ToList());
        }

        // GET: Artist/Details/5
        public ActionResult Details(string name)
        {
            Artist artist = _db.Artists.Find(name);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // GET: Artist/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artist/Create
        [HttpPost]
        public ActionResult Create(Artist artistToCreate)
        {
            if (!ModelState.IsValid)
                return View();

            _db.Artists.Add(artistToCreate);

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Artist/Edit/5
        public ActionResult Edit(string name)
        {
            return View();
        }

        // POST: Artist/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Artist artist)
        {
            if (!ModelState.IsValid)
                return View();

            _db.Entry(artist).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Artist/Delete/5
        public ActionResult Delete(string name)
        {
            Artist artist = _db.Artists.Find(name);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (!ModelState.IsValid)
                return View();

            Artist artist = _db.Artists.Find(id);
            _db.Artists.Remove(artist);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
