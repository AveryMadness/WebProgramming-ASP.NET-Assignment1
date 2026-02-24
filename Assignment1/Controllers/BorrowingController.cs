using System;
using Assignment1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    [ApiController]
    [Route("api/borrowings")]
    public class BorrowingController : ControllerBase
    {
        // GET: api/borrowings
        [HttpGet]
        public IActionResult GetAllBorrowings()
        {
            return Ok(new
            {
                success = true,
                message = "Retrieved all borrowings",
                data = new[]
                {
                    new
                    {
                        id = 1,
                        readerId = 1,
                        readerName = "John Doe",
                        bookId = 1,
                        bookTitle = "The Great Gatsby",
                        borrowDate = DateTime.UtcNow.AddDays(-10),
                        dueDate = DateTime.UtcNow.AddDays(4),
                        status = "Active"
                    },
                    new
                    {
                        id = 2,
                        readerId = 2,
                        readerName = "Jane Smith",
                        bookId = 2,
                        bookTitle = "1984",
                        borrowDate = DateTime.UtcNow.AddDays(-20),
                        dueDate = DateTime.UtcNow.AddDays(-6),
                        status = "Overdue"
                    }
                }
            });
        }

        // GET: api/borrowings/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetBorrowingById(int id)
        {
            return Ok(new
            {
                success = true,
                message = $"Retrieved borrowing with id {id}",
                data = new
                {
                    id,
                    readerId = 1,
                    readerName = "John Doe",
                    bookId = 1,
                    bookTitle = "The Great Gatsby",
                    borrowDate = DateTime.UtcNow.AddDays(-10),
                    dueDate = DateTime.UtcNow.AddDays(4),
                    returnDate = (DateTime?)null,
                    status = "Active",
                    overdueCharge = 0.00
                }
            });
        }

        // POST: api/borrowings
        [HttpPost]
        public IActionResult CreateBorrowing([FromBody] Borrowing borrowing)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            return Created("/api/borrowings/3", new
            {
                success = true,
                message = "Borrowing created successfully",
                data = new
                {
                    id = 3,
                    borrowing.ReaderId,
                    borrowing.BookId,
                    borrowDate = borrowing.BorrowDate,
                    dueDate = borrowing.DueDate,
                    status = borrowing.Status
                }
            });
        }

        // PUT: api/borrowings/{id}
        [HttpPut("{id:int}")]
        public IActionResult UpdateBorrowing(int id, [FromBody] Borrowing borrowing)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            return Ok(new
            {
                success = true,
                message = $"Borrowing with id {id} updated successfully",
                data = new
                {
                    id,
                    dueDate = borrowing.DueDate,
                    updated = DateTime.UtcNow
                }
            });
        }

        // DELETE: api/borrowings/{id}
        [HttpDelete("{id:int}")]
        public IActionResult DeleteBorrowing(int id)
        {
            return Ok(new
            {
                success = true,
                message = $"Borrowing with id {id} cancelled successfully"
            });
        }
    }
}


