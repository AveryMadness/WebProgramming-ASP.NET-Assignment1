using Assignment1.Models;
using Assignment1.Repositories;
using Assignment1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    public class ReaderController : Controller
    {
        private readonly ReaderRepository _readerRepository;
        private readonly SessionService _sessionService;

        public ReaderController(ReaderRepository readerRepository, SessionService sessionService)
        {
            _readerRepository = readerRepository;
            _sessionService = sessionService;
        }

        public IActionResult Index()
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var readers = _readerRepository.GetAll();
            return View(readers);
        }

        public IActionResult Details(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var reader = _readerRepository.GetById(id);
            if (reader == null) return NotFound();
            return View(reader);
        }

        public IActionResult Create()
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Reader reader)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            if (ModelState.IsValid)
            {
                _readerRepository.Add(reader);
                return RedirectToAction("Index");
            }
            return View(reader);
        }

        public IActionResult Edit(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var reader = _readerRepository.GetById(id);
            if (reader == null) return NotFound();
            return View(reader);
        }

        [HttpPost]
        public IActionResult Edit(int id, Reader reader)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            if (ModelState.IsValid)
            {
                _readerRepository.Update(id, reader);
                return RedirectToAction("Index");
            }
            return View(reader);
        }

        public IActionResult Delete(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            var reader = _readerRepository.GetById(id);
            if (reader == null) return NotFound();
            return View(reader);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_sessionService.IsLoggedIn()) return RedirectToAction("Login", "Auth");
            _readerRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
