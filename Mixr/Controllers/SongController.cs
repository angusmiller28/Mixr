using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomatedTellerMachine.Models;

namespace AutomatedTellerMachine.Controllers
{
    public class SongController : Controller
    {
        private MixrDBEntities _db = new MixrDBEntities();

        // GET: Song
        public ActionResult Index()
        {
            return View(_db.Songs.ToList());
        }

        // GET: Song/Details/5
        public ActionResult Details(int id)
        {
            Song song = _db.Songs.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        // GET: Song/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Song/Create 
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id")] Song songToCreate)

        {

            if (!ModelState.IsValid)
                return View();

            _db.Songs.Add(songToCreate);

            _db.SaveChanges();

            return RedirectToAction("Index");

        } 

        // GET: Song/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Song/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Song song)
        {
            if (!ModelState.IsValid)
                return View();

            _db.Entry(song).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Song/Delete/5
        public ActionResult Delete(int id)
        {
            Song song = _db.Songs.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        // POST: Song/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (!ModelState.IsValid)
                return View();

            Song song = _db.Songs.Find(id);
            _db.Songs.Remove(song);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
