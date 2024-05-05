using _6_modul_Test.Models;
using Microsoft.EntityFrameworkCore;

namespace _6_modul_Test.Data
{
    public class LabraryDbContext : DbContext
    {
        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Book> Books { get; set; }

        public LabraryDbContext(DbContextOptions<LabraryDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .ToTable("Author");

            modelBuilder.Entity<Author>()
                .HasKey(a => a.AuthorId);

            modelBuilder.Entity<Author>()
                .Property(a => a.FullName)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Author>()
                .Property(a => a.Birthday)
                .IsRequired()
                .HasColumnType("datetime");


            modelBuilder.Entity<Author>()
                .HasMany(a => a.books)
                .WithOne(a => a.Author)
                .HasForeignKey(a => a.AuthorId);

            modelBuilder.Entity<Book>()
                .ToTable("Book");

            modelBuilder.Entity<Book>()
                .HasKey(b => b.BookId);

            modelBuilder.Entity<Book>()
                .Property(b => b.Descriptions)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .IsRequired();

            modelBuilder.Entity<Category>()
                .ToTable("Category");

            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.books)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
