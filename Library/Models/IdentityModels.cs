using System.Collections.Generic;
using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace Library.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [Display(Name = "Joined on")]
        [DataType(DataType.Date)]
        public DateTime DateJoined { get; set; } = DateTime.UtcNow;

        [Display(Name = "Profile Picture")]
        public string ProfilePictureUrl { get; set; }
        public bool IsApproved { get; set; } = true;
        public bool IsBlocked { get; set; } = false;
        public DateTime? BlockedUntil { get; set; } = null;

        public virtual ICollection<Borrow> Borrows { get; set; }
        public virtual ICollection<SearchHistory> SearchHistories { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookFIle> BookFiles { get; set; }
        public DbSet<BookTag> BookTags { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<WaitList> WaitLists { get; set; }
    }
}