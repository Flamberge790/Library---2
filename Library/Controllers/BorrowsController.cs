using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Library.Models;
using Microsoft.AspNet.Identity;

namespace Library.Controllers
{
    public class BorrowsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Borrows
        [Authorize(Roles = "admin,employee,user")]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var borrows = db.Borrows
                            .Include(b => b.Book)
                            .Include(b => b.User)
                            .Where(b => b.UserId == userId);
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
        [Authorize(Roles = "admin,employee")]
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
        [Authorize(Roles = "admin,employee")]
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
        [Authorize(Roles = "admin,employee")]
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
            borrow.Status = LoanStatus.Available;
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
        [Authorize(Roles = "admin,employee,user")]
        public ActionResult CreateBorrow(int bookId)
        {
            var userId = User.Identity.GetUserId();
            var book = db.Books.Find(bookId);

            if (book.Stock > 0)
            {
                var borrow = new Borrow
                {
                    UserId = userId,
                    BookId = bookId,
                    BorrowDate = DateTime.UtcNow,
                    ReturnDate = null,
                    Status = LoanStatus.Borrowed
                };
                book.Stock -= 1;
                db.Borrows.Add(borrow);
                db.SaveChanges();
            }
            else
            {
                var borrow = new Borrow
                {
                    UserId = userId,
                    BookId = bookId,
                    BorrowDate = DateTime.UtcNow,
                    ReturnDate = null,
                    Status = LoanStatus.Queued
                };
                db.Borrows.Add(borrow);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin,employee,user")]
        public ActionResult RemoveBorrow(int bookId, int borrowId)
        {
            var book = db.Books.Find(bookId);
            Borrow borrow = db.Borrows.Find(borrowId);
            if (borrow.Status == LoanStatus.Borrowed)
            {
                book.Stock += 1;
            }
            borrow.Status = LoanStatus.Available;
            db.Borrows.Remove(borrow);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /*
        public string Createur()
        {
            IdentityManager im = new IdentityManager();
            im.CreateRole("User");
            im.CreateRole("Employee");
            im.CreateRole("Admin");
            //im.AddUserToRoleByUsername("dawid.dz1337@gmail.com", "user");
            string result;
            return "OK";
        }
        */
    }
}
