using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment1.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "Staff";

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
