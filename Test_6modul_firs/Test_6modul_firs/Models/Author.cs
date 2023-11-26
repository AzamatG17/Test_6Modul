namespace Test_6modul_firs.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FullName { get; set; }
        public DateOnly Birthday { get; set; }
        public virtual ICollection<Book> books { get; set; }
    }
}
