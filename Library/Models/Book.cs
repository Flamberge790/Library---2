namespace YourNamespace.Models
{
    public class Book
    {
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "Title")]
        public string Title { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "Author")]
        public string Author { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "Description")]
        public string Description { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "Release Date")]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public System.DateTime PublicationDate { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "Price")]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        public decimal Price { get; set; }

        [System.ComponentModel.DataAnnotations.Display(Name = "Cover")]
        public string ImageUrl { get; set; }
    }
}
