using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Author")]
        public string Author { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Table of Contents")]
        public string TableOfContents { get; set; }

        [Display(Name = "ISBN")]
        public string ISBN { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public ICollection<BookTag> BookTags { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public System.DateTime PublicationDate { get; set; }

        [Display(Name = "Cover")]
        public string BookCoverUrl { get; set; }
        public virtual ICollection<BookFIle> Files { get; set; }

        [Display(Name = "Stock")]
        public int Stock { get; set; }
        public virtual ICollection<WaitList> Waitlist { get; set; }
    }
}
