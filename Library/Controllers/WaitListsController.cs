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
    public class WaitListsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WaitLists
        public ActionResult Index()
        {
            var waitLists = db.WaitLists.Include(w => w.Book).Include(w => w.User);
            return View(waitLists.ToList());
        }

        // GET: WaitLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WaitList waitList = db.WaitLists.Find(id);
            if (waitList == null)
            {
                return HttpNotFound();
            }
            return View(waitList);
        }

        // GET: WaitLists/Create
        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "ProfilePictureUrl");
            return View();
        }

        // POST: WaitLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,BookId,EnqueuedAt")] WaitList waitList)
        {
            if (ModelState.IsValid)
            {
                db.WaitLists.Add(waitList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", waitList.BookId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "ProfilePictureUrl", waitList.UserId);
            return View(waitList);
        }

        // GET: WaitLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WaitList waitList = db.WaitLists.Find(id);
            if (waitList == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", waitList.BookId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "ProfilePictureUrl", waitList.UserId);
            return View(waitList);
        }

        // POST: WaitLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,BookId,EnqueuedAt")] WaitList waitList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(waitList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", waitList.BookId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "ProfilePictureUrl", waitList.UserId);
            return View(waitList);
        }

        // GET: WaitLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WaitList waitList = db.WaitLists.Find(id);
            if (waitList == null)
            {
                return HttpNotFound();
            }
            return View(waitList);
        }

        // POST: WaitLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WaitList waitList = db.WaitLists.Find(id);
            db.WaitLists.Remove(waitList);
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
