using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Library.Models;

namespace Library.Controllers
{
    public class BookFIlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BookFIles
        public ActionResult Index()
        {
            var bookFiles = db.BookFiles.Include(b => b.Book);
            return View(bookFiles.ToList());
        }

        // GET: BookFIles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookFIle bookFIle = db.BookFiles.Find(id);
            if (bookFIle == null)
            {
                return HttpNotFound();
            }
            return View(bookFIle);
        }

        // GET: BookFIles/Create
        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title");
            return View();
        }

        // POST: BookFIles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FileName,Description,BookId")] BookFIle bookFIle)
        {
            if (ModelState.IsValid)
            {
                db.BookFiles.Add(bookFIle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", bookFIle.BookId);
            return View(bookFIle);
        }

        // GET: BookFIles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookFIle bookFIle = db.BookFiles.Find(id);
            if (bookFIle == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", bookFIle.BookId);
            return View(bookFIle);
        }

        // POST: BookFIles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FileName,Description,BookId")] BookFIle bookFIle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookFIle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", bookFIle.BookId);
            return View(bookFIle);
        }

        // GET: BookFIles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookFIle bookFIle = db.BookFiles.Find(id);
            if (bookFIle == null)
            {
                return HttpNotFound();
            }
            return View(bookFIle);
        }

        // POST: BookFIles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookFIle bookFIle = db.BookFiles.Find(id);
            db.BookFiles.Remove(bookFIle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
