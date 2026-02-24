using System;
using Assignment1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        // GET: api/books
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            return Ok(new
            {
                success = true,
                message = "Retrieved all books (from BookController)",
                data = new[]
                {
                    new { id = 1, title = "The Great Gatsby", author = "F. Scott Fitzgerald", available = true },
                    new { id = 2, title = "1984", author = "George Orwell", available = true },
                    new { id = 3, title = "To Kill a Mockingbird", author = "Harper Lee", available = false }
                }
            });
        }

        // GET: api/books/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetBookById(int id)
        {
            return Ok(new
            {
                success = true,
                message = $"Retrieved book with id {id}",
                data = new
                {
                    id,
                    title = "The Great Gatsby",
                    author = "F. Scott Fitzgerald",
                    isbn = "978-0-7432-7356-5",
                    category = "Fiction",
                    quantity = 5,
                    available = 3
                }
            });
        }

        // POST: api/books
        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            // No real persistence, just return some information
            return Created("/api/books/4", new
            {
                success = true,
                message = "Book created successfully",
                data = new
                {
                    id = 4,
                    book.Title,
                    book.Author,
                    created = DateTime.UtcNow
                }
            });
        }

        // PUT: api/books/{id}
        [HttpPut("{id:int}")]
        public IActionResult UpdateBook(int id, [FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            return Ok(new
            {
                success = true,
                message = $"Book with id {id} updated successfully",
                data = new
                {
                    id,
                    title = book.Title,
                    author = book.Author,
                    updated = DateTime.UtcNow
                }
            });
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id:int}")]
        public IActionResult DeleteBook(int id)
        {
            return Ok(new
            {
                success = true,
                message = $"Book with id {id} deleted successfully"
            });
        }
    }
}


