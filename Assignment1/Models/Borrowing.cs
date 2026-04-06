using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment1.Models
{
    public class Borrowing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReaderId { get; set; }

        [ForeignKey("ReaderId")]
        public virtual Reader? Reader { get; set; }

        [Required]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }

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

        [Column(TypeName = "decimal(10,2)")]
        public decimal OverdueCharge { get; set; }
    }
}
