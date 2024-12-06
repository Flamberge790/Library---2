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
    public class SearchHistoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SearchHistories
        public ActionResult Index()
        {
            var searchHistories = db.SearchHistories.Include(s => s.User);
            return View(searchHistories.ToList());
        }

        // GET: SearchHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearchHistory searchHistory = db.SearchHistories.Find(id);
            if (searchHistory == null)
            {
                return HttpNotFound();
            }
            return View(searchHistory);
        }

        // GET: SearchHistories/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "ProfilePictureUrl");
            return View();
        }

        // POST: SearchHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Query,SearchDate")] SearchHistory searchHistory)
        {
            if (ModelState.IsValid)
            {
                db.SearchHistories.Add(searchHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "ProfilePictureUrl", searchHistory.UserId);
            return View(searchHistory);
        }

        // GET: SearchHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearchHistory searchHistory = db.SearchHistories.Find(id);
            if (searchHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "ProfilePictureUrl", searchHistory.UserId);
            return View(searchHistory);
        }

        // POST: SearchHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Query,SearchDate")] SearchHistory searchHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(searchHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "ProfilePictureUrl", searchHistory.UserId);
            return View(searchHistory);
        }

        // GET: SearchHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearchHistory searchHistory = db.SearchHistories.Find(id);
            if (searchHistory == null)
            {
                return HttpNotFound();
            }
            return View(searchHistory);
        }

        // POST: SearchHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SearchHistory searchHistory = db.SearchHistories.Find(id);
            db.SearchHistories.Remove(searchHistory);
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
