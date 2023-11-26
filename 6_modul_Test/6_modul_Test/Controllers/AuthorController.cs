using _6_modul_Test.Data;
using _6_modul_Test.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace _6_modul_Test.Controllers
{
    public class AuthorController : Controller
    {
        private readonly LabraryDbContext _context;

        public AuthorController(LabraryDbContext context)
        {
            _context = context;
        }

        // GET: AuthorController
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            if (sortOrder == null)
            {
                sortOrder = "";
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["FirstNameSort"] = sortOrder == "firstName_asc" ? "firstName_desc" : "firstName_asc";
            ViewData["BirthdaySort"] = sortOrder == "Birthday_asc" ? "Birthday_desc" : "Birthday_asc";

            var authors = _context.Author.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                authors = authors
                    .Where(s => s.FullName
                    .ToLower()
                    .Contains(searchString
                    .ToLower()))
                    .AsQueryable();
            }

            authors = sortOrder switch
            {
                "firstName_asc" => authors.OrderBy(a => a.FullName),
                "firstName_desc" => authors.OrderByDescending(a => a.FullName),
                "Birthday_asc" => authors.OrderBy(a => a.Birthday),
                "Birthday_desc" => authors.OrderByDescending(a => a.Birthday),
                _ => authors.OrderBy(a => a.FullName)
            };

            return View(authors);
        }



        // GET: AuthorController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _context.Author.FirstOrDefault(a => a.AuthorId == id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: AuthorController/Create

        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,FullName,Birthday")] Author author)
        {
            if (author != null)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: AuthorController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Author == null)
            {
                return NotFound();
            }

            var author = await _context.Author.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,FullName,Birthday")] Author author)
        {
            if (id != author.AuthorId)
            {
                return NotFound();
            }

            if (author != null)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthorId))
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
            return View(author);
        }


        // GET: AuthorController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.Author == null)
            {
                return NotFound();
            }

            var author = _context.Author.FirstOrDefault(a => a.AuthorId == id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: AuthorController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Author == null)
            {
                return Problem("Entity set 'LibraryDbContext.Authors'  is null.");
            }

            var author = await _context.Author.FindAsync(id);
            if (author != null)
            {
                _context.Author.Remove(author);
            }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _context.Author.Any(a => a.AuthorId == id);
        }
    }
}
