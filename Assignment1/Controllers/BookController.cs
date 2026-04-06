using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment1.Data;
using Assignment1.Models;
using Assignment1.Services;

namespace Assignment1.Controllers
{
    public class BookController : Controller
    {
        private readonly LMSDbContext _context;
        private readonly SessionService _sessionService;

        public BookController(LMSDbContext context, SessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public IActionResult Index(string searchString, string category)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");

            var books = from b in _context.Books select b;

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString) || b.Author.Contains(searchString) || b.Isbn.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(category))
            {
                books = books.Where(b => b.Category == category);
            }

            ViewBag.Categories = _context.Books.Select(b => b.Category).Distinct().Where(c => c != null).ToList();
            ViewBag.SearchString = searchString;
            ViewBag.SelectedCategory = category;

            return View(books.ToList());
        }

        public IActionResult Details(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var book = _context.Books.Include(b => b.Borrowings).ThenInclude(br => br.Reader).FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            return View(book);
        }

        public IActionResult Create()
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,Author,Isbn,Category,TotalCopies,PublishedDate")] Book book)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            if (ModelState.IsValid)
            {
                book.AvailableCopies = book.TotalCopies;
                _context.Books.Add(book);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        public IActionResult Edit(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var book = _context.Books.Find(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,Author,Isbn,Category,TotalCopies,AvailableCopies,PublishedDate")] Book book)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            if (id != book.Id) return NotFound();
            
            if (ModelState.IsValid)
            {
                _context.Update(book);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        public IActionResult Delete(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var book = _context.Books.Find(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
