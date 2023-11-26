using _6_modul_Test.Data;
using _6_modul_Test.Migrations;
using _6_modul_Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace _6_modul_Test.Controllers
{
    public class BookController : Controller
    {
        private readonly LabraryDbContext _context;

        public BookController(LabraryDbContext context)
        {
            _context = context;
        }
        // GET: BookController
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            if (sortOrder == null)
            {
                sortOrder = "";
            }

            const string DefaultSortOrder = "description_asc";
            const string DescriptionSortKey = "DescriptionSort";
            const string PriceSortKey = "PriceSort";
            const string CategorySortKey = "CategorySort";

            sortOrder ??= DefaultSortOrder;

            ViewData["CurrentSort"] = sortOrder;
            ViewData["DescriptionSortKey"] = sortOrder == "description_asc" ? "description_desc" : "description_asc";
            ViewData["PriceSortKey"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewData["CategorySortKey"] = sortOrder == "category_asc" ? "category_desc" : "category_asc";

            var books = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Descriptions.ToLower().Contains(searchString.ToLower()));
            }

            books = SortBooks(books, sortOrder);

            var sortedBooks = await books.Include(b => b.Category).ToListAsync();

            return View(sortedBooks);
        }

        private IQueryable<Book> SortBooks(IQueryable<Book> books, string sortOrder)
        {
            return sortOrder switch
            {
                "description_asc" => books.OrderBy(b => b.Descriptions),
                "description_desc" => books.OrderByDescending(b => b.Descriptions),
                "price_asc" => books.OrderBy(b => b.Price),
                "price_desc" => books.OrderByDescending(b => b.Price),
                "category_asc" => books.OrderBy(b => b.Category.Name),
                "category_desc" => books.OrderByDescending(b => b.Category.Name),
                _ => books.OrderBy(b => b.Descriptions) 
            };
        }


        // GET: BookController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: BookController/Create


        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            ViewData["AuthorId"] = new SelectList(_context.Category, "Id", "FullName");

            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Descriptions, Price, CategoryId, AuthorId")] Book book)
        {
            if (book != null)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", book.CategoryId);
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FullName", book.AuthorId);
            return View(book);
        }

        
        public async Task<ActionResult> Edit(int id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            var categories = await _context.Category
        .Select(c => new SelectListItem
        {
            Value = c.CategoryId.ToString(),
            Text = c.Name
        })
        .ToListAsync();

            ViewBag.CategoryId = categories;

            var authors = await _context.Author
                .Select(a => new SelectListItem
                {
                    Value = a.AuthorId.ToString(),
                    Text = a.FullName
                })
                .ToListAsync();

            ViewBag.AuthorId = authors;
            return View(book);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("BookId,Descriptions,Price,CategoryId,AuthorId")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Books.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", book.CategoryId);
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "FullName", book.AuthorId);
            return View(book);
        }

        // GET: BookController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FirstOrDefaultAsync(m => m.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            
            if (_context.Books == null)
            {
                return Problem("Entity set 'LibraryDbContext.Authors'  is null.");
            }

            var book = await _context.Books.FindAsync(id);

            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
