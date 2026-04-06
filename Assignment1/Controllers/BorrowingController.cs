using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment1.Data;
using Assignment1.Models;
using Assignment1.Services;

namespace Assignment1.Controllers
{
    public class BorrowingController : Controller
    {
        private readonly LMSDbContext _context;
        private readonly SessionService _sessionService;
        private const decimal DailyOverdueRate = 2.00m;

        public BorrowingController(LMSDbContext context, SessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public IActionResult Index(string searchString, string status, DateTime? startDate, DateTime? endDate)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");

            var borrowings = _context.Borrowings.Include(b => b.Book).Include(b => b.Reader).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                borrowings = borrowings.Where(b => 
                    (b.Book != null && b.Book.Title.Contains(searchString)) || 
                    (b.Reader != null && b.Reader.Name.Contains(searchString)));
            }

            if (!string.IsNullOrEmpty(status))
            {
                borrowings = borrowings.Where(b => b.Status == status);
            }

            if (startDate.HasValue)
            {
                borrowings = borrowings.Where(b => b.BorrowDate >= startDate);
            }

            if (endDate.HasValue)
            {
                borrowings = borrowings.Where(b => b.BorrowDate <= endDate);
            }

            ViewBag.StatusList = new List<string> { "Active", "Returned", "Overdue", "Cancelled" };
            ViewBag.SearchString = searchString;
            ViewBag.SelectedStatus = status;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(borrowings.ToList());
        }

        public IActionResult Details(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var borrowing = _context.Borrowings.Include(b => b.Book).Include(b => b.Reader).FirstOrDefault(b => b.Id == id);
            if (borrowing == null) return NotFound();
            return View(borrowing);
        }

        public IActionResult Create()
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            ViewBag.Books = _context.Books.Where(b => b.AvailableCopies > 0).ToList();
            ViewBag.Readers = _context.Readers.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ReaderId,BookId,DueDate")] Borrowing borrowing)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            
            var book = _context.Books.Find(borrowing.BookId);
            if (book == null || book.AvailableCopies <= 0)
            {
                ModelState.AddModelError("", "Book is not available");
                ViewBag.Books = _context.Books.Where(b => b.AvailableCopies > 0).ToList();
                ViewBag.Readers = _context.Readers.ToList();
                return View(borrowing);
            }

            if (ModelState.IsValid)
            {
                borrowing.BorrowDate = DateTime.UtcNow;
                borrowing.Status = "Active";
                _context.Borrowings.Add(borrowing);
                
                book.AvailableCopies--;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Books = _context.Books.Where(b => b.AvailableCopies > 0).ToList();
            ViewBag.Readers = _context.Readers.ToList();
            return View(borrowing);
        }

        public IActionResult Edit(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var borrowing = _context.Borrowings.Find(id);
            if (borrowing == null) return NotFound();
            ViewBag.Books = _context.Books.ToList();
            ViewBag.Readers = _context.Readers.ToList();
            return View(borrowing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,ReaderId,BookId,BorrowDate,DueDate,ReturnDate,Status,OverdueCharge")] Borrowing borrowing)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            if (id != borrowing.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(borrowing);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Books = _context.Books.ToList();
            ViewBag.Readers = _context.Readers.ToList();
            return View(borrowing);
        }

        public IActionResult Delete(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var borrowing = _context.Borrowings.Include(b => b.Book).Include(b => b.Reader).FirstOrDefault(b => b.Id == id);
            if (borrowing == null) return NotFound();
            return View(borrowing);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var borrowing = _context.Borrowings.Find(id);
            if (borrowing != null)
            {
                var book = _context.Books.Find(borrowing.BookId);
                if (book != null)
                {
                    book.AvailableCopies++;
                }
                _context.Borrowings.Remove(borrowing);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Return(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var borrowing = _context.Borrowings.Find(id);
            if (borrowing == null) return NotFound();

            borrowing.ReturnDate = DateTime.UtcNow;
            if (borrowing.ReturnDate > borrowing.DueDate)
            {
                int overdueDays = (borrowing.ReturnDate.Value - borrowing.DueDate).Days;
                borrowing.OverdueCharge = overdueDays * DailyOverdueRate;
                borrowing.Status = "Overdue";
            }
            else
            {
                borrowing.Status = "Returned";
                borrowing.OverdueCharge = 0;
            }

            var book = _context.Books.Find(borrowing.BookId);
            if (book != null)
            {
                book.AvailableCopies++;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Overdue()
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            
            foreach (var borrowing in _context.Borrowings.Where(b => b.Status == "Active").ToList())
            {
                if (DateTime.UtcNow > borrowing.DueDate)
                {
                    int overdueDays = (DateTime.UtcNow - borrowing.DueDate).Days;
                    borrowing.OverdueCharge = overdueDays * DailyOverdueRate;
                    borrowing.Status = "Overdue";
                }
            }
            _context.SaveChanges();

            var overdue = _context.Borrowings.Include(b => b.Book).Include(b => b.Reader).Where(b => b.Status == "Overdue").ToList();
            return View(overdue);
        }

        public IActionResult Reports()
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            ViewBag.Books = _context.Books.ToList();
            ViewBag.Readers = _context.Readers.ToList();
            ViewBag.StatusList = new List<string> { "Active", "Returned", "Overdue", "Cancelled" };
            return View();
        }

        [HttpPost]
        public IActionResult GenerateReport(DateTime? startDate, DateTime? endDate, int? bookId, int? readerId, string status)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");

            var borrowings = _context.Borrowings.Include(b => b.Book).Include(b => b.Reader).AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
            {
                borrowings = borrowings.Where(b => b.BorrowDate >= startDate && b.BorrowDate <= endDate);
            }
            if (bookId.HasValue)
            {
                borrowings = borrowings.Where(b => b.BookId == bookId);
            }
            if (readerId.HasValue)
            {
                borrowings = borrowings.Where(b => b.ReaderId == readerId);
            }
            if (!string.IsNullOrEmpty(status))
            {
                borrowings = borrowings.Where(b => b.Status == status);
            }

            ViewBag.Books = _context.Books.ToList();
            ViewBag.Readers = _context.Readers.ToList();
            ViewBag.StatusList = new List<string> { "Active", "Returned", "Overdue", "Cancelled" };
            return View("Index", borrowings.ToList());
        }
    }
}
