using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Assignment1.Models;

namespace Assignment1.Services
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetSession(User user)
        {
            var json = JsonSerializer.Serialize(user);
            _httpContextAccessor.HttpContext?.Session.SetString("User", json);
        }

        public User? GetSession()
        {
            var json = _httpContextAccessor.HttpContext?.Session.GetString("User");
            if (string.IsNullOrEmpty(json)) return null;
            return JsonSerializer.Deserialize<User>(json);
        }

        public void ClearSession()
        {
            _httpContextAccessor.HttpContext?.Session.Remove("User");
        }

        public bool IsLoggedIn()
        {
            return GetSession() != null;
        }
    }
}
