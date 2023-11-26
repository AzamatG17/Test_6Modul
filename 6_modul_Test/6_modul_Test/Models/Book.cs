namespace _6_modul_Test.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Descriptions { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
