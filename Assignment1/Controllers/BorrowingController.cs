using Assignment1.Models;
using Assignment1.Repositories;
using Assignment1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    public class BorrowingController : Controller
    {
        private readonly BorrowingRepository _borrowingRepository;
        private readonly BookRepository _bookRepository;
        private readonly ReaderRepository _readerRepository;
        private readonly SessionService _sessionService;

        public BorrowingController(
            BorrowingRepository borrowingRepository,
            BookRepository bookRepository,
            ReaderRepository readerRepository,
            SessionService sessionService)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _readerRepository = readerRepository;
            _sessionService = sessionService;
        }

        public IActionResult Index()
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var borrowings = _borrowingRepository.GetAll();
            foreach (var b in borrowings)
            {
                b.BookId = b.BookId;
                b.ReaderId = b.ReaderId;
            }
            ViewBag.Books = _bookRepository.GetAll();
            ViewBag.Readers = _readerRepository.GetAll();
            return View(borrowings);
        }

        public IActionResult Details(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var borrowing = _borrowingRepository.GetById(id);
            if (borrowing == null) return NotFound();
            
            var book = _bookRepository.GetById(borrowing.BookId);
            var reader = _readerRepository.GetById(borrowing.ReaderId);
            ViewBag.BookTitle = book?.Title;
            ViewBag.ReaderName = reader?.Name;
            return View(borrowing);
        }

        public IActionResult Create()
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            ViewBag.Books = _bookRepository.GetAll().Where(b => b.AvailableCopies > 0).ToList();
            ViewBag.Readers = _readerRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Borrowing borrowing)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            if (ModelState.IsValid)
            {
                var book = _bookRepository.GetById(borrowing.BookId);
                if (book == null || book.AvailableCopies <= 0)
                {
                    ModelState.AddModelError("", "Book is not available");
                    ViewBag.Books = _bookRepository.GetAll().Where(b => b.AvailableCopies > 0).ToList();
                    ViewBag.Readers = _readerRepository.GetAll();
                    return View(borrowing);
                }

                borrowing.BorrowDate = DateTime.UtcNow;
                borrowing.Status = "Active";
                _borrowingRepository.Add(borrowing);
                _bookRepository.UpdateAvailability(borrowing.BookId, -1);
                return RedirectToAction("Index");
            }
            ViewBag.Books = _bookRepository.GetAll().Where(b => b.AvailableCopies > 0).ToList();
            ViewBag.Readers = _readerRepository.GetAll();
            return View(borrowing);
        }

        public IActionResult Edit(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var borrowing = _borrowingRepository.GetById(id);
            if (borrowing == null) return NotFound();
            
            ViewBag.Books = _bookRepository.GetAll();
            ViewBag.Readers = _readerRepository.GetAll();
            return View(borrowing);
        }

        [HttpPost]
        public IActionResult Edit(int id, Borrowing borrowing)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            if (ModelState.IsValid)
            {
                var existing = _borrowingRepository.GetById(id);
                if (existing == null) return NotFound();

                if (existing.BookId != borrowing.BookId)
                {
                    _bookRepository.UpdateAvailability(existing.BookId, 1);
                    var newBook = _bookRepository.GetById(borrowing.BookId);
                    if (newBook == null || newBook.AvailableCopies <= 0)
                    {
                        ModelState.AddModelError("", "Selected book is not available");
                        ViewBag.Books = _bookRepository.GetAll();
                        ViewBag.Readers = _readerRepository.GetAll();
                        return View(borrowing);
                    }
                    _bookRepository.UpdateAvailability(borrowing.BookId, -1);
                }

                _borrowingRepository.Update(id, borrowing);
                return RedirectToAction("Index");
            }
            ViewBag.Books = _bookRepository.GetAll();
            ViewBag.Readers = _readerRepository.GetAll();
            return View(borrowing);
        }

        public IActionResult Delete(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var borrowing = _borrowingRepository.GetById(id);
            if (borrowing == null) return NotFound();
            
            var book = _bookRepository.GetById(borrowing.BookId);
            var reader = _readerRepository.GetById(borrowing.ReaderId);
            ViewBag.BookTitle = book?.Title;
            ViewBag.ReaderName = reader?.Name;
            return View(borrowing);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var borrowing = _borrowingRepository.GetById(id);
            if (borrowing != null)
            {
                _bookRepository.UpdateAvailability(borrowing.BookId, 1);
                _borrowingRepository.Delete(id);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Return(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var borrowing = _borrowingRepository.GetById(id);
            if (borrowing == null) return NotFound();
            
            borrowing.ReturnDate = DateTime.UtcNow;
            if (borrowing.ReturnDate > borrowing.DueDate)
            {
                int overdueDays = ((DateTime)borrowing.ReturnDate - borrowing.DueDate).Days;
                borrowing.OverdueCharge = overdueDays * 2;
                borrowing.Status = "Overdue";
            }
            else
            {
                borrowing.Status = "Returned";
                borrowing.OverdueCharge = 0;
            }
            
            _bookRepository.UpdateAvailability(borrowing.BookId, 1);
            _borrowingRepository.Update(id, borrowing);
            return RedirectToAction("Index");
        }

        public IActionResult Overdue()
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            _borrowingRepository.CalculateOverdueCharges();
            var overdue = _borrowingRepository.GetOverdue();
            ViewBag.Books = _bookRepository.GetAll();
            ViewBag.Readers = _readerRepository.GetAll();
            return View(overdue);
        }

        public IActionResult Reports()
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            ViewBag.Books = _bookRepository.GetAll();
            ViewBag.Readers = _readerRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Reports(DateTime? startDate, DateTime? endDate, int? bookId, int? readerId, string status)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            
            var borrowings = _borrowingRepository.GetAll();
            
            if (startDate.HasValue && endDate.HasValue)
            {
                borrowings = borrowings.Where(b => b.BorrowDate >= startDate && b.BorrowDate <= endDate).ToList();
            }
            if (bookId.HasValue)
            {
                borrowings = borrowings.Where(b => b.BookId == bookId).ToList();
            }
            if (readerId.HasValue)
            {
                borrowings = borrowings.Where(b => b.ReaderId == readerId).ToList();
            }
            if (!string.IsNullOrEmpty(status))
            {
                borrowings = borrowings.Where(b => b.Status == status).ToList();
            }

            ViewBag.Books = _bookRepository.GetAll();
            ViewBag.Readers = _readerRepository.GetAll();
            return View("Index", borrowings);
        }
    }
}
