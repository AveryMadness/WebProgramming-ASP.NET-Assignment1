using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment1.Data;
using Assignment1.Services;

namespace Assignment1.Controllers
{
    public class DashboardController : Controller
    {
        private readonly LMSDbContext _context;
        private readonly SessionService _sessionService;

        public DashboardController(LMSDbContext context, SessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public IActionResult Index()
        {
            if (!_sessionService.IsLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = _sessionService.GetSession();
            ViewBag.User = user;

            ViewBag.TotalBooks = _context.Books.Count();
            ViewBag.AvailableBooks = _context.Books.Sum(b => b.AvailableCopies);
            ViewBag.TotalReaders = _context.Readers.Count();
            ViewBag.ActiveBorrowings = _context.Borrowings.Where(b => b.Status == "Active").Count();
            ViewBag.OverdueBorrowings = _context.Borrowings.Where(b => b.Status == "Overdue").Count();
            ViewBag.TotalBorrowings = _context.Borrowings.Count();
            ViewBag.ReturnedBorrowings = _context.Borrowings.Where(b => b.Status == "Returned").Count();

            return View();
        }
    }
}
