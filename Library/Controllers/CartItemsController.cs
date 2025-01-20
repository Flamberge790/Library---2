using Library.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class CartItemsController : Controller
    {
        // GET: CartItems
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var cartItems = db.CartItems.Where(c => c.UserId == userId).ToList();
            return View(cartItems);
        }

        [HttpPost]
        public ActionResult AddToCart(int bookId)
        {
            var userId = User.Identity.GetUserId();
            var existingItem = db.CartItems.FirstOrDefault(c => c.BookId == bookId && c.UserId == userId);
            if (existingItem == null)
            {
                var cartItem = new CartItem
                {
                    BookId = bookId,
                    UserId = userId
                };
                db.CartItems.Add(cartItem);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int cartItemId)
        {
            var cartItem = db.CartItems.Find(cartItemId);
            if (cartItem != null)
            {
                db.CartItems.Remove(cartItem);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}