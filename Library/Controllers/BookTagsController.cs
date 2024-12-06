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
    public class BookTagsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BookTags
        public ActionResult Index()
        {
            var bookTags = db.BookTags.Include(b => b.Book).Include(b => b.Tag);
            return View(bookTags.ToList());
        }

        // GET: BookTags/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookTag bookTag = db.BookTags.Find(id);
            if (bookTag == null)
            {
                return HttpNotFound();
            }
            return View(bookTag);
        }

        // GET: BookTags/Create
        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title");
            ViewBag.TagId = new SelectList(db.Tags, "Id", "Name");
            return View();
        }

        // POST: BookTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BookId,TagId")] BookTag bookTag)
        {
            if (ModelState.IsValid)
            {
                db.BookTags.Add(bookTag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", bookTag.BookId);
            ViewBag.TagId = new SelectList(db.Tags, "Id", "Name", bookTag.TagId);
            return View(bookTag);
        }

        // GET: BookTags/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookTag bookTag = db.BookTags.Find(id);
            if (bookTag == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", bookTag.BookId);
            ViewBag.TagId = new SelectList(db.Tags, "Id", "Name", bookTag.TagId);
            return View(bookTag);
        }

        // POST: BookTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BookId,TagId")] BookTag bookTag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookTag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", bookTag.BookId);
            ViewBag.TagId = new SelectList(db.Tags, "Id", "Name", bookTag.TagId);
            return View(bookTag);
        }

        // GET: BookTags/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookTag bookTag = db.BookTags.Find(id);
            if (bookTag == null)
            {
                return HttpNotFound();
            }
            return View(bookTag);
        }

        // POST: BookTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookTag bookTag = db.BookTags.Find(id);
            db.BookTags.Remove(bookTag);
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
