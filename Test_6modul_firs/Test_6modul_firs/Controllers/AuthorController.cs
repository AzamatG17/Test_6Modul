using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test_6modul_firs.Data;
using Test_6modul_firs.Models;

namespace Test_6modul_firs.Controllers
{
    public class AuthorController : Controller
    {
        private readonly LabraryDbContext _context;

        public AuthorController(LabraryDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var authors = await _context.Author.ToListAsync();

            return View(authors);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Author author)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Author.Add(author);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Автор успешно добавлен!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Ошибка при добавлении автора: {ex.Message}";
                }
            }

            return View(author);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var author = await _context.Author.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Author.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            _context.Author.Remove(author);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var author = await _context.Author.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Author.Update(author);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        public async Task<IActionResult> Details(int id)
        {
            var author = await _context.Author.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost]
        public async Task<IActionResult> Details(int id, [Bind("AuthorId,Name")] Author author)
        {
            if (id != author.AuthorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(author);
        }


    }
}
