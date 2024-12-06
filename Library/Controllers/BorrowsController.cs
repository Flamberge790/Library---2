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
    public class BorrowsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Borrows
        public ActionResult Index()
        {
            var borrows = db.Borrows.Include(b => b.Book).Include(b => b.User);
            return View(borrows.ToList());
        }

        // GET: Borrows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = db.Borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            return View(borrow);
        }

        // GET: Borrows/Create
        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "ProfilePictureUrl");
            return View();
        }

        // POST: Borrows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,BookId,BorrowDate,ReturnDate,Status")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                db.Borrows.Add(borrow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", borrow.BookId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "ProfilePictureUrl", borrow.UserId);
            return View(borrow);
        }

        // GET: Borrows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = db.Borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", borrow.BookId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "ProfilePictureUrl", borrow.UserId);
            return View(borrow);
        }

        // POST: Borrows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,BookId,BorrowDate,ReturnDate,Status")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(borrow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", borrow.BookId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "ProfilePictureUrl", borrow.UserId);
            return View(borrow);
        }

        // GET: Borrows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Borrow borrow = db.Borrows.Find(id);
            if (borrow == null)
            {
                return HttpNotFound();
            }
            return View(borrow);
        }

        // POST: Borrows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Borrow borrow = db.Borrows.Find(id);
            db.Borrows.Remove(borrow);
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
