using LibraryManagementSystem.Models;
using LibraryManagementSystem.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Service
{
    public class LibraryService : ILibraryService
    {

        private readonly ApplicationDbContext _context;

        public LibraryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task BorrowBookAsync(int bookId, int patronId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var borrowingRecord = new BorrowingRecord
                    {
                        BookId = bookId,
                        PatronId = patronId,
                        BorrowDate = DateTime.Now
                    };

                    _context.BorrowingRecords.Add(borrowingRecord);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        public async Task ReturnBookAsync(int recordId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var record = await _context.BorrowingRecords.FindAsync(recordId);
                    if (record != null)
                    {
                        record.ReturnDate = DateTime.Now;
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }

}
