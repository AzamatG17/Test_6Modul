using _6_modul_Test.Data;
using _6_modul_Test.Models;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace _6_modul_Test.Extansion
{
    public static class DatabaseSeeder
    {
        private static Faker _faker = new Faker("ru");

        public static void SeedDatabase(this IServiceCollection _, IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<LabraryDbContext>>();
            using var context = new LabraryDbContext(options);

            CreateAuthor(context);
            CreateCategoryBooks(context);
            CreateBooks(context);
        }

        private static void CreateAuthor(LabraryDbContext context)
        {
            if (context.Author.Any()) return;

            List<Author> authorList = new List<Author>();

            Random rand = new Random();

            for (int i = 0; i < 50; i++)
            {
                int year = rand.Next(1970, 2023);
                int month = rand.Next(1, 13);
                int maxDay = DateTime.DaysInMonth(year, month);
                int day = rand.Next(1, maxDay + 1);

                authorList.Add(new Author
                {
                    FullName = _faker.Name.FullName(),
                    Birthday = new DateTime(year, month, day)
                });
            }

            context.Author.AddRange(authorList);
            context.SaveChanges();
        }


        private static void CreateCategoryBooks(LabraryDbContext context)
        {
            if (context.Category.Any()) return;

            List<Category> categories = new List<Category>();

            for (int i = 0; i < 10; i++)
            {
                categories.Add(new Category
                {
                    Name = _faker.Random.Word()
                });
            }
            context.Category.AddRange(categories);
            context.SaveChanges();
        }

        private static void CreateBooks(LabraryDbContext context)
        {
            if (context.Books.Any()) return;

            List<Book> books = new List<Book>();

            Random random = new Random();

            var author = context.Author.Select(a => a.AuthorId).ToList();
            var category = context.Category.Select(a => a.CategoryId).ToList();

            for (int i = 0; i < 400; i++)
            {
                books.Add(new Book
                {
                    Descriptions = _faker.Lorem.Sentence(),
                    Price = (decimal)random.Next(10, 1000),
                    AuthorId = author[random.Next(author.Count)],
                    CategoryId = category[random.Next(category.Count)]
                });
            }

            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}
