using System.Diagnostics.CodeAnalysis;

namespace LibraryManagementSystem.Models
{
    public class BorrowingRecord
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int PatronId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // Navigation properties
        public Book Book { get; set; }
        public Patron Patron { get; set; }
    }
}
