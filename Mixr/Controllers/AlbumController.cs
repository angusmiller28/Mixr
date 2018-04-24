using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomatedTellerMachine.Models;
using System.IO;


namespace AutomatedTellerMachine.Controllers
{
    public class AlbumController : Controller
    {
        private MixrDBEntities _db = new MixrDBEntities();

        public void SetNoticationBar()
        {
            ViewBag.Notification = null;
            ViewBag.SuccessNotificationSet = false;
            ViewBag.ErrorNotificationSet = false;

            // set notification bar
            if (TempData["success"] != null)
            {
                ViewBag.SucessNotificationSet = true;
                ViewBag.Notification = TempData["success"].ToString();
            }
            else if (TempData["error"] != null)
            {
                ViewBag.ErrorNotificationSet = true;
                ViewBag.Notification = TempData["error"].ToString();
            }
        }

        public bool IsImage(HttpPostedFileBase file)
        {
            // File Handling: Valid file type is an image
            if (file.ContentType.ToLower() != "image/jpg" && file.ContentType.ToLower() != "image/jpeg" && file.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            return true;
        }

        // GET: Album
        public ActionResult Index()
        {

            SetNoticationBar();

            return View(_db.Albums.ToList());
        }

        // GET: Album/Details/5
        public ActionResult Details(int id)
        {
            Album album = _db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // GET: Album/Create
        public ActionResult Create()
        {
            SetNoticationBar();

            return View();
        }

        // POST: Album/Create
        [HttpPost]
        public ActionResult Create(Album albumToCreate, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
                return View();

            string path = "";
            TempData["success"] = null;
            TempData["error"] = null;

            if (file != null && file.ContentLength > 0)
            {

                // If file is not and image
                if (!IsImage(file))
                {
                    TempData["error"] = "Invalid File Type. Please use either jpg, jpeg or png.";
                    RedirectToAction("Create");
                }


                try
                {
                    // Save image to uploads folder
                    path = Path.Combine(Server.MapPath("/Uploads/Albums"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);

                    // Upload Album
                    string filename = Path.GetFileName(file.FileName);

                    albumToCreate.SetCoverImage(filename);
                    _db.Albums.Add(albumToCreate);
                    _db.SaveChanges();

                    TempData["success"] = "Album uploaded successfully.";
                }
                catch (Exception ex)
                {
                    TempData["error"] = "ERROR:" + ex.Message.ToString();
                }

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "You have not specified a file.";
                return View();
            }
        }

        // GET: Album/Edit/5
        public ActionResult Edit(int id)
        {
            SetNoticationBar();

            // Fill input fields with the data of the edit album
            Album album = _db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }

            return View(album);
        }

        // POST: Album/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Album album, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
                return View();

            string path = "";
            TempData["success"] = null;
            TempData["error"] = null;

            // File Handling
            if (file != null && file.ContentLength > 0)
            {
                // If file is not and image
                if (!IsImage(file))
                {
                    TempData["error"] = "Invalid File Type. Please use either jpg, jpeg or png.";
                    RedirectToAction("Create");
                }

                
                try
                {
                    // Remove Old File
                    path = Path.Combine(Server.MapPath("/Uploads/Albums"),
                                               album.album_cover);
                    FileInfo fileToRemove = new FileInfo(path);
                    fileToRemove.Delete();

                    // Add New File
                    path = Path.Combine(Server.MapPath("/Uploads/Albums"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                }
                catch (Exception ex)
                {
                    TempData["error"] = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                ViewBag.Message = "You have not specified a file.";
                return View();
            }

            string filename = Path.GetFileName(file.FileName);
      
            // Edit Album
            album.SetCoverImage(filename);
            _db.Entry(album).State = EntityState.Modified;
            _db.SaveChanges();

            TempData["success"] = "Album edited successfully";

            return RedirectToAction("Index");
        }

        // GET: Album/Delete/5
        public ActionResult Delete(int id)
        {
            Album album = _db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Album/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (!ModelState.IsValid)
                return View();

            Album album = _db.Albums.Find(id);
            _db.Albums.Remove(album);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
