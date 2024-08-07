using System.Diagnostics.CodeAnalysis;

namespace LibraryManagementSystem.Models
{
    public class Patron
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactInformation { get; set; }

        // Navigation property for related BorrowingRecords
        public ICollection<BorrowingRecord> BorrowingRecords { get; set; }
    }
}
