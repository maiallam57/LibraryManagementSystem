using System.Diagnostics.CodeAnalysis;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }
        public string ISBN { get; set; }

        // Navigation property for related BorrowingRecords
        public ICollection<BorrowingRecord> BorrowingRecords { get; set; }

    }
}
