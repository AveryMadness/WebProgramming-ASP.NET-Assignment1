using Microsoft.EntityFrameworkCore;
using Assignment1.Models;

namespace Assignment1.Data
{
    public class LMSDbContext : DbContext
    {
        public LMSDbContext(DbContextOptions<LMSDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
                entity.Property(b => b.Author).IsRequired().HasMaxLength(150);
                entity.Property(b => b.Isbn).IsRequired().HasMaxLength(20);
                entity.Property(b => b.Category).HasMaxLength(100);
                entity.HasMany(b => b.Borrowings)
                      .WithOne(br => br.Book)
                      .HasForeignKey(br => br.BookId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Reader>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(150);
                entity.Property(r => r.Email).IsRequired().HasMaxLength(200);
                entity.Property(r => r.Phone).HasMaxLength(20);
                entity.Property(r => r.Address).HasMaxLength(250);
                entity.Property(r => r.MembershipType).IsRequired().HasMaxLength(50);
                entity.HasMany(r => r.Borrowings)
                      .WithOne(br => br.Reader)
                      .HasForeignKey(br => br.ReaderId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Borrowing>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Status).IsRequired().HasMaxLength(30);
                entity.Property(b => b.OverdueCharge).HasColumnType("decimal(10,2)");
                entity.HasOne(br => br.Book)
                      .WithMany(b => b.Borrowings)
                      .HasForeignKey(br => br.BookId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(br => br.Reader)
                      .WithMany(r => r.Borrowings)
                      .HasForeignKey(br => br.ReaderId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Isbn = "978-0-7432-7356-5", Category = "Fiction", TotalCopies = 5, AvailableCopies = 3, PublishedDate = new DateTime(1925, 4, 10) },
                new Book { Id = 2, Title = "1984", Author = "George Orwell", Isbn = "978-0-452-28423-4", Category = "Science Fiction", TotalCopies = 3, AvailableCopies = 2, PublishedDate = new DateTime(1949, 6, 8) },
                new Book { Id = 3, Title = "To Kill a Mockingbird", Author = "Harper Lee", Isbn = "978-0-06-112008-4", Category = "Fiction", TotalCopies = 4, AvailableCopies = 0, PublishedDate = new DateTime(1960, 7, 11) },
                new Book { Id = 4, Title = "Pride and Prejudice", Author = "Jane Austen", Isbn = "978-0-14-143951-8", Category = "Romance", TotalCopies = 2, AvailableCopies = 2, PublishedDate = new DateTime(1813, 1, 28) },
                new Book { Id = 5, Title = "The Catcher in the Rye", Author = "J.D. Salinger", Isbn = "978-0-316-76948-0", Category = "Fiction", TotalCopies = 3, AvailableCopies = 1, PublishedDate = new DateTime(1951, 7, 16) }
            );

            modelBuilder.Entity<Reader>().HasData(
                new Reader { Id = 1, Name = "John Doe", Email = "john@email.com", Phone = "555-0101", Address = "123 Main St", MembershipType = "Premium", RegisteredDate = DateTime.UtcNow.AddMonths(-6) },
                new Reader { Id = 2, Name = "Jane Smith", Email = "jane@email.com", Phone = "555-0102", Address = "456 Oak Ave", MembershipType = "Standard", RegisteredDate = DateTime.UtcNow.AddMonths(-3) },
                new Reader { Id = 3, Name = "Bob Johnson", Email = "bob@email.com", Phone = "555-0103", Address = "789 Pine Rd", MembershipType = "Standard", RegisteredDate = DateTime.UtcNow.AddMonths(-1) }
            );
        }
    }
}
