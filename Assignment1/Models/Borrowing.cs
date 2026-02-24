using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment1.Models
{
    //Represents a borrowing transaction between a reader and a book.
    public class Borrowing
    {
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ReaderId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int BookId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Active";

        [Range(0, 10000)]
        [DataType(DataType.Currency)]
        public decimal OverdueCharge { get; set; }
    }
}


