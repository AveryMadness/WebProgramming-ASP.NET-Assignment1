using System.Text.Json;
using Assignment1.Models;
using Assignment1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly SessionService _sessionService;

        public AuthController(AuthService authService, SessionService sessionService)
        {
            _authService = authService;
            _sessionService = sessionService;
        }

        public IActionResult Login()
        {
            if (_sessionService.IsLoggedIn())
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var result = _authService.Login(username, password);
            if (result.Success)
            {
                _sessionService.SetSession(result.User!);
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Error = result.Message;
            return View();
        }

        public IActionResult Register()
        {
            if (_sessionService.IsLoggedIn())
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string email, string password, string role = "Staff")
        {
            var result = _authService.Register(username, email, password, role);
            if (result.Success)
            {
                return RedirectToAction("Login");
            }
            ViewBag.Error = result.Message;
            return View();
        }

        public IActionResult Logout()
        {
            _sessionService.ClearSession();
            return RedirectToAction("Login");
        }
    }
}
