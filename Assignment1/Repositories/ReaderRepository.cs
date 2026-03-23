using System;
using System.Collections.Generic;
using Assignment1.Models;

namespace Assignment1.Repositories
{
    public class ReaderRepository
    {
        private static List<Reader> _readers = new List<Reader>
        {
            new Reader { Id = 1, Name = "John Doe", Email = "john@email.com", Phone = "555-0101", Address = "123 Main St", MembershipType = "Premium", RegisteredDate = DateTime.UtcNow.AddMonths(-6) },
            new Reader { Id = 2, Name = "Jane Smith", Email = "jane@email.com", Phone = "555-0102", Address = "456 Oak Ave", MembershipType = "Standard", RegisteredDate = DateTime.UtcNow.AddMonths(-3) },
            new Reader { Id = 3, Name = "Bob Johnson", Email = "bob@email.com", Phone = "555-0103", Address = "789 Pine Rd", MembershipType = "Standard", RegisteredDate = DateTime.UtcNow.AddMonths(-1) }
        };

        private static int _nextId = 4;

        public List<Reader> GetAll() => _readers;

        public Reader? GetById(int id) => _readers.Find(r => r.Id == id);

        public Reader Add(Reader reader)
        {
            reader.Id = _nextId++;
            reader.RegisteredDate = DateTime.UtcNow;
            _readers.Add(reader);
            return reader;
        }

        public bool Update(int id, Reader updatedReader)
        {
            int index = _readers.FindIndex(r => r.Id == id);
            if (index == -1) return false;
            updatedReader.Id = id;
            _readers[index] = updatedReader;
            return true;
        }

        public bool Delete(int id)
        {
            return _readers.RemoveAll(r => r.Id == id) > 0;
        }
    }
}
