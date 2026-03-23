using Assignment1.Models;
using Assignment1.Repositories;
using Assignment1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    public class BookController : Controller
    {
        private readonly BookRepository _bookRepository;
        private readonly SessionService _sessionService;

        public BookController(BookRepository bookRepository, SessionService sessionService)
        {
            _bookRepository = bookRepository;
            _sessionService = sessionService;
        }

        public IActionResult Index()
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var books = _bookRepository.GetAll();
            return View(books);
        }

        public IActionResult Details(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var book = _bookRepository.GetById(id);
            if (book == null) return NotFound();
            return View(book);
        }

        public IActionResult Create()
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            if (ModelState.IsValid)
            {
                book.AvailableCopies = book.TotalCopies;
                _bookRepository.Add(book);
                return RedirectToAction("Index");
            }
            return View(book);
        }

        public IActionResult Edit(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var book = _bookRepository.GetById(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(int id, Book book)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            if (ModelState.IsValid)
            {
                var existingBook = _bookRepository.GetById(id);
                if (existingBook == null) return NotFound();

                int diff = book.TotalCopies - existingBook.TotalCopies;
                book.AvailableCopies = existingBook.AvailableCopies + diff;
                if (book.AvailableCopies < 0) book.AvailableCopies = 0;
                if (book.AvailableCopies > book.TotalCopies) book.AvailableCopies = book.TotalCopies;

                _bookRepository.Update(id, book);
                return RedirectToAction("Index");
            }
            return View(book);
        }

        public IActionResult Delete(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var book = _bookRepository.GetById(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            _bookRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
