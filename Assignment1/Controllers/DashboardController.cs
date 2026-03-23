using Assignment1.Repositories;
using Assignment1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    public class DashboardController : Controller
    {
        private readonly BookRepository _bookRepository;
        private readonly ReaderRepository _readerRepository;
        private readonly BorrowingRepository _borrowingRepository;
        private readonly SessionService _sessionService;

        public DashboardController(
            BookRepository bookRepository,
            ReaderRepository readerRepository,
            BorrowingRepository borrowingRepository,
            SessionService sessionService)
        {
            _bookRepository = bookRepository;
            _readerRepository = readerRepository;
            _borrowingRepository = borrowingRepository;
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

            ViewBag.TotalBooks = _bookRepository.GetAll().Count;
            ViewBag.AvailableBooks = _bookRepository.GetAll().Sum(b => b.AvailableCopies);
            ViewBag.TotalReaders = _readerRepository.GetAll().Count;
            ViewBag.ActiveBorrowings = _borrowingRepository.GetByStatus("Active").Count;
            ViewBag.OverdueBorrowings = _borrowingRepository.GetOverdue().Count;

            return View();
        }
    }
}
