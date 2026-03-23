using System;
using System.Collections.Generic;
using System.Linq;
using Assignment1.Models;

namespace Assignment1.Repositories
{
    public class BorrowingRepository
    {
        private static List<Borrowing> _borrowings = new List<Borrowing>
        {
            new Borrowing { Id = 1, ReaderId = 1, BookId = 1, BorrowDate = DateTime.UtcNow.AddDays(-10), DueDate = DateTime.UtcNow.AddDays(4), Status = "Active", OverdueCharge = 0 },
            new Borrowing { Id = 2, ReaderId = 2, BookId = 2, BorrowDate = DateTime.UtcNow.AddDays(-20), DueDate = DateTime.UtcNow.AddDays(-6), Status = "Overdue", OverdueCharge = 12 }
        };

        private static int _nextId = 3;
        private const decimal DailyOverdueRate = 2.00m;

        public List<Borrowing> GetAll() => _borrowings;

        public Borrowing? GetById(int id) => _borrowings.Find(b => b.Id == id);

        public List<Borrowing> GetByReaderId(int readerId) => _borrowings.Where(b => b.ReaderId == readerId).ToList();

        public List<Borrowing> GetByBookId(int bookId) => _borrowings.Where(b => b.BookId == bookId).ToList();

        public List<Borrowing> GetOverdue() => _borrowings.Where(b => b.Status == "Overdue").ToList();

        public Borrowing Add(Borrowing borrowing)
        {
            borrowing.Id = _nextId++;
            _borrowings.Add(borrowing);
            return borrowing;
        }

        public bool Update(int id, Borrowing updatedBorrowing)
        {
            int index = _borrowings.FindIndex(b => b.Id == id);
            if (index == -1) return false;
            updatedBorrowing.Id = id;
            _borrowings[index] = updatedBorrowing;
            return true;
        }

        public bool Delete(int id)
        {
            return _borrowings.RemoveAll(b => b.Id == id) > 0;
        }

        public bool ReturnBook(int id, DateTime returnDate)
        {
            var borrowing = GetById(id);
            if (borrowing == null) return false;

            borrowing.ReturnDate = returnDate;
            
            if (returnDate > borrowing.DueDate)
            {
                int overdueDays = (returnDate - borrowing.DueDate).Days;
                borrowing.OverdueCharge = overdueDays * DailyOverdueRate;
                borrowing.Status = "Overdue";
            }
            else
            {
                borrowing.Status = "Returned";
                borrowing.OverdueCharge = 0;
            }

            return true;
        }

        public void CalculateOverdueCharges()
        {
            foreach (var borrowing in _borrowings.Where(b => b.Status == "Active"))
            {
                if (DateTime.UtcNow > borrowing.DueDate)
                {
                    int overdueDays = (DateTime.UtcNow - borrowing.DueDate).Days;
                    borrowing.OverdueCharge = overdueDays * DailyOverdueRate;
                    borrowing.Status = "Overdue";
                }
            }
        }

        public List<Borrowing> GetByDateRange(DateTime start, DateTime end)
        {
            return _borrowings.Where(b => b.BorrowDate >= start && b.BorrowDate <= end).ToList();
        }

        public List<Borrowing> GetByStatus(string status)
        {
            return _borrowings.Where(b => b.Status == status).ToList();
        }
    }
}
