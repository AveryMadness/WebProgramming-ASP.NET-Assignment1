using System;
using Assignment1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{
    [ApiController]
    [Route("api/readers")]
    public class ReaderController : ControllerBase
    {
        // GET: api/readers
        [HttpGet]
        public IActionResult GetAllReaders()
        {
            return Ok(new
            {
                success = true,
                message = "Retrieved all readers",
                data = new[]
                {
                    new { id = 1, name = "John Doe", email = "john@email.com", membershipType = "Premium" },
                    new { id = 2, name = "Jane Smith", email = "jane@email.com", membershipType = "Standard" },
                    new { id = 3, name = "Bob Johnson", email = "bob@email.com", membershipType = "Standard" }
                }
            });
        }

        // GET: api/readers/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetReaderById(int id)
        {
            return Ok(new
            {
                success = true,
                message = $"Retrieved reader with id {id}",
                data = new
                {
                    id,
                    name = "John Doe",
                    email = "john@email.com",
                    phone = "555-0101",
                    address = "123 Main St",
                    membershipType = "Premium",
                    registeredDate = DateTime.UtcNow.AddMonths(-6)
                }
            });
        }

        // POST: api/readers
        [HttpPost]
        public IActionResult CreateReader([FromBody] Reader reader)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            return Created("/api/readers/4", new
            {
                success = true,
                message = "Reader created successfully",
                data = new
                {
                    id = 4,
                    reader.Name,
                    reader.Email,
                    registeredDate = DateTime.UtcNow
                }
            });
        }

        // PUT: api/readers/{id}
        [HttpPut("{id:int}")]
        public IActionResult UpdateReader(int id, [FromBody] Reader reader)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            return Ok(new
            {
                success = true,
                message = $"Reader with id {id} updated successfully",
                data = new
                {
                    id,
                    name = reader.Name,
                    email = reader.Email,
                    updated = DateTime.UtcNow
                }
            });
        }

        // DELETE: api/readers/{id}
        [HttpDelete("{id:int}")]
        public IActionResult DeleteReader(int id)
        {
            return Ok(new
            {
                success = true,
                message = $"Reader with id {id} deleted successfully"
            });
        }
    }
}


