namespace _6_modul_Test.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FullName { get; set; }

        public DateTime Birthday { get; set; }
        public virtual ICollection<Book> books { get; set; }
    }
}
