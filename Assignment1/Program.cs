var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();


//GET all books.
app.MapGet("/api/books", () =>
{
    return Results.Ok(new
    {
        success = true,
        message = "Retrieved all books",
        data = new[]
        {
            new { id = 1, title = "The Great Gatsby", author = "F. Scott Fitzgerald", available = true },
            new { id = 2, title = "1984", author = "George Orwell", available = true },
            new { id = 3, title = "To Kill a Mockingbird", author = "Harper Lee", available = false }
        }
    });
});


//GET book by id
app.MapGet("/api/books/{id}", (int id) =>
{
    return Results.Ok(new
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
});

//POST to create a new book
app.MapPost("/api/books", (object book) =>
{
    return Results.Created("/api/books/4", new
    {
        success = true,
        message = "Book created successfully",
        data = new
        {
            id = 4,
            title = "New Book",
            author = "New Author",
            created = DateTime.Now
        }
    });
});

//PUT to update a book
app.MapPut("/api/books/{id}", (int id, object book) =>
{
    return Results.Ok(new
    {
        success = true,
        message = $"Book with id {id} updated successfully",
        data = new
        {
            id,
            title = "Updated Book Title",
            author = "Updated Author",
            updated = DateTime.Now
        }
    });
});

//DELETE a book
app.MapDelete("/api/books/{id}", (int id) =>
{
    return Results.Ok(new
    {
        success = true,
        message = $"Book with id {id} deleted successfully"
    });
});

//GET book availability
app.MapGet("/api/books/{id}/availability", (int id) =>
{
    return Results.Ok(new
    {
        success = true,
        data = new
        {
            bookId = id,
            totalCopies = 5,
            availableCopies = 3,
            borrowedCopies = 2
        }
    });
});

//GET readers
app.MapGet("/api/readers", () =>
{
    return Results.Ok(new
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
});

//GET reader by id
app.MapGet("/api/readers/{id}", (int id) =>
{
    return Results.Ok(new
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
            registeredDate = DateTime.Now.AddMonths(-6)
        }
    });
});

//POST to create a new reader
app.MapPost("/api/readers", (object reader) =>
{
    return Results.Created("/api/readers/4", new
    {
        success = true,
        message = "Reader created successfully",
        data = new
        {
            id = 4,
            name = "New Reader",
            email = "newreader@email.com",
            registeredDate = DateTime.Now
        }
    });
});

//PUT to update a user
app.MapPut("/api/readers/{id}", (int id, object reader) =>
{
    return Results.Ok(new
    {
        success = true,
        message = $"Reader with id {id} updated successfully",
        data = new
        {
            id,
            name = "Updated Reader Name",
            email = "updated@email.com",
            updated = DateTime.Now
        }
    });
});

//DELETE a reader
app.MapDelete("/api/readers/{id}", (int id) =>
{
    return Results.Ok(new
    {
        success = true,
        message = $"Reader with id {id} deleted successfully"
    });
});

//GET a readers borrowings
app.MapGet("/api/readers/{id}/borrowings", (int id) =>
{
    return Results.Ok(new
    {
        success = true,
        data = new[]
        {
            new
            {
                borrowingId = 1,
                bookTitle = "The Great Gatsby",
                borrowDate = DateTime.Now.AddDays(-10),
                dueDate = DateTime.Now.AddDays(4),
                status = "Active"
            },
            new
            {
                borrowingId = 2,
                bookTitle = "1984",
                borrowDate = DateTime.Now.AddDays(-25),
                dueDate = DateTime.Now.AddDays(-11),
                status = "Overdue"
            }
        }
    });
});

//GET a readers overdues
app.MapGet("/api/readers/{id}/overdues", (int id) =>
{
    return Results.Ok(new
    {
        success = true,
        data = new[]
        {
            new
            {
                borrowingId = 2,
                bookTitle = "1984",
                dueDate = DateTime.Now.AddDays(-11),
                daysOverdue = 11,
                overdueCharge = 5.50
            }
        },
        totalOverdueCharge = 5.50
    });
});

//GET all borrowings
app.MapGet("/api/borrowings", () =>
{
    return Results.Ok(new
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
                borrowDate = DateTime.Now.AddDays(-10),
                dueDate = DateTime.Now.AddDays(4),
                status = "Active"
            },
            new
            {
                id = 2,
                readerId = 2,
                readerName = "Jane Smith",
                bookId = 2,
                bookTitle = "1984",
                borrowDate = DateTime.Now.AddDays(-20),
                dueDate = DateTime.Now.AddDays(-6),
                status = "Overdue"
            }
        }
    });
});

//GET a borrowing by its id
app.MapGet("/api/borrowings/{id}", (int id) =>
{
    return Results.Ok(new
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
            borrowDate = DateTime.Now.AddDays(-10),
            dueDate = DateTime.Now.AddDays(4),
            returnDate = (DateTime?)null,
            status = "Active",
            overdueCharge = 0.00
        }
    });
});

//POST to create a borrowing
app.MapPost("/api/borrowings", (object borrowing) =>
{
    return Results.Created("/api/borrowings/3", new
    {
        success = true,
        message = "Borrowing created successfully",
        data = new
        {
            id = 3,
            readerId = 1,
            readerName = "John Doe",
            bookId = 3,
            bookTitle = "To Kill a Mockingbird",
            borrowDate = DateTime.Now,
            dueDate = DateTime.Now.AddDays(14),
            status = "Active"
        }
    });
});

//PUT to update a borrowing, (can be used to extend due date)
app.MapPut("/api/borrowings/{id}", (int id, object borrowing) =>
{
    return Results.Ok(new
    {
        success = true,
        message = $"Borrowing with id {id} updated successfully",
        data = new
        {
            id,
            dueDate = DateTime.Now.AddDays(21),
            updated = DateTime.Now
        }
    });
});

//PUT to return a book
app.MapPut("/api/borrowings/{id}/return", (int id) =>
{
    return Results.Ok(new
    {
        success = true,
        message = "Book returned successfully",
        data = new
        {
            id,
            returnDate = DateTime.Now,
            status = "Returned",
            overdueCharge = 0.00
        }
    });
});

//DELETE to cancel a borrowing
app.MapDelete("/api/borrowings/{id}", (int id) =>
{
    return Results.Ok(new
    {
        success = true,
        message = $"Borrowing with id {id} cancelled successfully"
    });
});

//GET overdue borrowings
app.MapGet("/api/borrowings/overdue", () =>
{
    return Results.Ok(new
    {
        success = true,
        data = new[]
        {
            new
            {
                id = 2,
                readerId = 2,
                readerName = "Jane Smith",
                bookTitle = "1984",
                dueDate = DateTime.Now.AddDays(-6),
                daysOverdue = 6,
                overdueCharge = 3.00
            }
        }
    });
});


//dashboard routes

//GET dashboard stats
app.MapGet("/api/dashboard/stats", () =>
{
    return Results.Ok(new
    {
        success = true,
        data = new
        {
            totalBooks = 150,
            availableBooks = 98,
            borrowedBooks = 52,
            totalReaders = 45,
            activeBorrowings = 52,
            overdueCount = 8
        }
    });
});

//GET borrowing reports
app.MapGet("/api/dashboard/reports/borrowings", () =>
{
    return Results.Ok(new
    {
        success = true,
        reportDate = DateTime.Now,
        data = new
        {
            totalBorrowings = 120,
            byStatus = new[]
            {
                new { status = "Active", count = 52 },
                new { status = "Returned", count = 60 },
                new { status = "Overdue", count = 8 }
            }
        }
    });
});

//GET book reports
app.MapGet("/api/dashboard/reports/books", () =>
{
    return Results.Ok(new
    {
        success = true,
        reportDate = DateTime.Now,
        data = new
        {
            byCategory = new[]
            {
                new { category = "Fiction", totalBooks = 80, availableBooks = 45 },
                new { category = "Non-Fiction", totalBooks = 50, availableBooks = 35 },
                new { category = "Science", totalBooks = 20, availableBooks = 18 }
            }
        }
    });
});

//GET reader reports
app.MapGet("/api/dashboard/reports/readers", () =>
{
    return Results.Ok(new
    {
        success = true,
        reportDate = DateTime.Now,
        data = new
        {
            totalReaders = 45,
            byMembershipType = new[]
            {
                new { membershipType = "Premium", count = 15 },
                new { membershipType = "Standard", count = 30 }
            }
        }
    });
});

//GET overdue reports
app.MapGet("/api/dashboard/reports/overdues", () =>
{
    return Results.Ok(new
    {
        success = true,
        reportDate = DateTime.Now,
        data = new
        {
            totalOverdue = 8,
            totalCharges = 24.50,
            overdueItems = new[]
            {
                new
                {
                    borrowingId = 2,
                    readerName = "Jane Smith",
                    bookTitle = "1984",
                    daysOverdue = 6,
                    overdueCharge = 3.00
                },
                new
                {
                    borrowingId = 5,
                    readerName = "Bob Johnson",
                    bookTitle = "Pride and Prejudice",
                    daysOverdue = 15,
                    overdueCharge = 7.50
                }
            }
        }
    });
});

app.Run();