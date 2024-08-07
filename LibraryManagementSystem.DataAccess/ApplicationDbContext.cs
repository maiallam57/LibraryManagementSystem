
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.DataAccess;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set up relationships and constraints if needed
        modelBuilder.Entity<BorrowingRecord>()
            .HasOne(b => b.Book)
            .WithMany(br => br.BorrowingRecords)
            .HasForeignKey(b => b.BookId);

        modelBuilder.Entity<BorrowingRecord>()
            .HasOne(p => p.Patron)
            .WithMany(br => br.BorrowingRecords)
            .HasForeignKey(p => p.PatronId);
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Patron> Patrons { get; set; }
    public DbSet<BorrowingRecord> BorrowingRecords { get; set; }
}
