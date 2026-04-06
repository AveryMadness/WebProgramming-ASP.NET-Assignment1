using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment1.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[0-9\-]+$", ErrorMessage = "ISBN must contain only digits and hyphens.")]
        public string Isbn { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Category { get; set; }

        [Range(0, int.MaxValue)]
        public int TotalCopies { get; set; }

        [Range(0, int.MaxValue)]
        public int AvailableCopies { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PublishedDate { get; set; }

        public virtual ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
    }
}
