using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment1.Models
{
    //Represents a library reader / member.
    public class Reader
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        [Required]
        [StringLength(50)]
        public string MembershipType { get; set; } = "Standard";

        [DataType(DataType.Date)]
        public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;
    }
}


