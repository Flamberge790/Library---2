using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Library.Models;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        public ActionResult Index(string searchQuery, string searchOperator = "||")
        {
            var books = db.Books.Include(b => b.Category);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                var terms = searchQuery.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if(searchOperator == "&&")
                {
                    foreach (var term in terms)
                    {
                        books = books.Where(b => 
                            b.Title.ToLower().Contains(term.ToLower()) ||
                            b.Author.ToLower().Contains(term.ToLower()) ||
                            b.ISBN.Contains(term)
                        );
                    }
                }
                else if (searchOperator == "||")
                {
                    foreach (var term in terms)
                    {
                        books = books.Where(b => terms.Any(t =>
                            b.Title.ToLower().Contains(t.ToLower()) ||
                            b.Author.ToLower().Contains(t.ToLower()) ||
                            b.ISBN.Contains(t)
                            )
                        );
                    }
                }
                else
                {
                    foreach (var term in terms)
                    {
                        books = books.Where(b => terms.All(t => 
                            !b.Title.ToLower().Contains(t.ToLower()) &&
                            !b.Author.ToLower().Contains(t.ToLower()) &&
                            !b.ISBN.Contains(t)
                            )
                        );
                    }
                }
            }
            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Book book = db.Books.Find(id);
            Book book = db.Books
                       .Include(b => b.BookTags.Select(bt => bt.Tag))
                       .FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Author,Description,TableOfContents,ISBN,CategoryId,PublicationDate,BookCoverUrl,Stock")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Author,Description,TableOfContents,ISBN,CategoryId,PublicationDate,BookCoverUrl,Stock")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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

        [HttpGet]
        public ActionResult AssignTag(int bookId)
        {
            var book = db.Books.Find(bookId);
            if (book == null)
            {
                return HttpNotFound();
            }

            ViewBag.Tags = new SelectList(db.Tags, "Id", "Name");
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignTag(int bookId, int tagId)
        {
            var existsBookTag = db.BookTags.FirstOrDefault(bt => bt.BookId == bookId && bt.TagId == tagId);

            if (existsBookTag == null)
            {
                BookTag newBookTag = new BookTag
                {
                    BookId = bookId,
                    TagId = tagId
                };

                db.BookTags.Add(newBookTag);
                db.SaveChanges();
            }
            //Debug.WriteLine($"BookId: {bookId}, TagId: {tagId}");
            return RedirectToAction("Details", "Books", new { id = bookId });
        }

    }
}
