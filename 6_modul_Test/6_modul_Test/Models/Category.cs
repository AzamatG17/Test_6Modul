namespace _6_modul_Test.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Book> books { get; set; }
    }
}
