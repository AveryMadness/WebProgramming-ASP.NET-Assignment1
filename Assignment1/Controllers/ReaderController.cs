using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment1.Data;
using Assignment1.Models;
using Assignment1.Services;

namespace Assignment1.Controllers
{
    public class ReaderController : Controller
    {
        private readonly LMSDbContext _context;
        private readonly SessionService _sessionService;

        public ReaderController(LMSDbContext context, SessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        public IActionResult Index(string searchString, string membershipType)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");

            var readers = from r in _context.Readers select r;

            if (!string.IsNullOrEmpty(searchString))
            {
                readers = readers.Where(r => r.Name.Contains(searchString) || r.Email.Contains(searchString) || (r.Phone != null && r.Phone.Contains(searchString)));
            }

            if (!string.IsNullOrEmpty(membershipType))
            {
                readers = readers.Where(r => r.MembershipType == membershipType);
            }

            ViewBag.MembershipTypes = new List<string> { "Standard", "Premium", "VIP" };
            ViewBag.SearchString = searchString;
            ViewBag.SelectedMembershipType = membershipType;

            return View(readers.ToList());
        }

        public IActionResult Details(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var reader = _context.Readers.Include(r => r.Borrowings).ThenInclude(b => b.Book).FirstOrDefault(r => r.Id == id);
            if (reader == null) return NotFound();
            return View(reader);
        }

        public IActionResult Create()
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Email,Phone,Address,MembershipType")] Reader reader)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            if (ModelState.IsValid)
            {
                reader.RegisteredDate = DateTime.UtcNow;
                _context.Readers.Add(reader);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reader);
        }

        public IActionResult Edit(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var reader = _context.Readers.Find(id);
            if (reader == null) return NotFound();
            return View(reader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Email,Phone,Address,MembershipType,RegisteredDate")] Reader reader)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            if (id != reader.Id) return NotFound();
            
            if (ModelState.IsValid)
            {
                _context.Update(reader);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reader);
        }

        public IActionResult Delete(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var reader = _context.Readers.Find(id);
            if (reader == null) return NotFound();
            return View(reader);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var reader = _context.Readers.Find(id);
            if (reader != null)
            {
                _context.Readers.Remove(reader);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
