using System;
using System.Collections.Generic;
using Assignment1.Models;

namespace Assignment1.Repositories
{
    public class BookRepository
    {
        private static List<Book> _books = new List<Book>
        {
            new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Isbn = "978-0-7432-7356-5", Category = "Fiction", TotalCopies = 5, AvailableCopies = 3, PublishedDate = new DateTime(1925, 4, 10) },
            new Book { Id = 2, Title = "1984", Author = "George Orwell", Isbn = "978-0-452-28423-4", Category = "Science Fiction", TotalCopies = 3, AvailableCopies = 2, PublishedDate = new DateTime(1949, 6, 8) },
            new Book { Id = 3, Title = "To Kill a Mockingbird", Author = "Harper Lee", Isbn = "978-0-06-112008-4", Category = "Fiction", TotalCopies = 4, AvailableCopies = 0, PublishedDate = new DateTime(1960, 7, 11) },
            new Book { Id = 4, Title = "Pride and Prejudice", Author = "Jane Austen", Isbn = "978-0-14-143951-8", Category = "Romance", TotalCopies = 2, AvailableCopies = 2, PublishedDate = new DateTime(1813, 1, 28) },
            new Book { Id = 5, Title = "The Catcher in the Rye", Author = "J.D. Salinger", Isbn = "978-0-316-76948-0", Category = "Fiction", TotalCopies = 3, AvailableCopies = 1, PublishedDate = new DateTime(1951, 7, 16) }
        };

        private static int _nextId = 6;

        public List<Book> GetAll() => _books;

        public Book? GetById(int id) => _books.Find(b => b.Id == id);

        public Book Add(Book book)
        {
            book.Id = _nextId++;
            _books.Add(book);
            return book;
        }

        public bool Update(int id, Book updatedBook)
        {
            int index = _books.FindIndex(b => b.Id == id);
            if (index == -1) return false;
            updatedBook.Id = id;
            _books[index] = updatedBook;
            return true;
        }

        public bool Delete(int id)
        {
            return _books.RemoveAll(b => b.Id == id) > 0;
        }

        public bool UpdateAvailability(int bookId, int change)
        {
            var book = GetById(bookId);
            if (book == null) return false;
            book.AvailableCopies += change;
            if (book.AvailableCopies < 0) book.AvailableCopies = 0;
            if (book.AvailableCopies > book.TotalCopies) book.AvailableCopies = book.TotalCopies;
            return true;
        }
    }
}
