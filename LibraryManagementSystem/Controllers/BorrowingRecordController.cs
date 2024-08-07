using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.DataAccess;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{

	public class BorrowingRecordController : BaseApiController
	{

		private readonly ApplicationDbContext _context;
		private readonly ILogger<BorrowingRecordController> _logger;
        public BorrowingRecordController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowingRecord>>> GetBorrowingRecords()
        {
            return await _context.BorrowingRecords
                .Include(br => br.Book)
                .Include(br => br.Patron)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowingRecord>> GetBorrowingRecord(int id)
        {
            var borrowingRecord = await _context.BorrowingRecords
                .Include(br => br.Book)
                .Include(br => br.Patron)
                .FirstOrDefaultAsync(br => br.Id == id);

            if (borrowingRecord == null)
            {
                return NotFound();
            }

            return borrowingRecord;
        }

        [HttpPost]
        public async Task<ActionResult<BorrowingRecord>> PostBorrowingRecord(BorrowingRecord borrowingRecord)
        {
            _context.BorrowingRecords.Add(borrowingRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBorrowingRecord), new { id = borrowingRecord.Id }, borrowingRecord);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBorrowingRecord(int id, BorrowingRecord borrowingRecord)
        {
            if (id != borrowingRecord.Id)
            {
                return BadRequest();
            }

            _context.Entry(borrowingRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowingRecordExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrowingRecord(int id)
        {
            var borrowingRecord = await _context.BorrowingRecords.FindAsync(id);
            if (borrowingRecord == null)
            {
                return NotFound();
            }

            _context.BorrowingRecords.Remove(borrowingRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BorrowingRecordExists(int id)
        {
            return _context.BorrowingRecords.Any(e => e.Id == id);
        }


    }
}