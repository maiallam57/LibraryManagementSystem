using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Service
{
    public interface ILibraryService
    {
        public Task AddBookAsync(Book book);

        public Task<List<Book>> GetAllBooksAsync();

        public Task BorrowBookAsync(int bookId, int patronId);

        public Task ReturnBookAsync(int recordId);
    }


}
