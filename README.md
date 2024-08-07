Sure! Here’s how you can implement declarative transaction management, write unit tests for API endpoints, and provIde clear documentation in a C# project using ASP.NET Core.

### Transaction Management using Entity Framework Core

In ASP.NET Core, you can use the `TransactionScope` class for transaction management. However, for simplicity and to align with typical practices, we'll rely on the transaction capabilities provIded by Entity Framework Core's `SaveChanges` method.

### Implementing Transaction Management

#### BorrowingRecordService with Transaction Management

```csharp
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

                // Additional operations can be included here within the same transaction

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

                    // Additional operations can be included here within the same transaction

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
```

### Testing API Endpoints

To write unit tests, we'll use `xUnit` and `Moq`. We need to mock the database context to test our controllers.

#### Install Packages

First, install the necessary NuGet packages:

```bash
dotnet add package xunit
dotnet add package Moq
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

#### Sample Unit Test for BookController

```csharp
public class BookControllerTests
{
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly BookController _controller;
    private readonly Mock<DbSet<Book>> _mockSet;

    public BookControllerTests()
    {
        _mockContext = new Mock<ApplicationDbContext>();
        _mockSet = new Mock<DbSet<Book>>();

        _mockContext.Setup(m => m.Books).Returns(_mockSet.Object);
        _controller = new BookController(_mockContext.Object);
    }

    [Fact]
    public async Task GetBooks_ReturnsAllBooks()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "Book1", Author = "Author1" },
            new Book { Id = 2, Title = "Book2", Author = "Author2" }
        }.AsQueryable();

        _mockSet.As<IQueryable<Book>>().Setup(m => m.ProvIder).Returns(books.ProvIder);
        _mockSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(books.Expression);
        _mockSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(books.ElementType);
        _mockSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(books.GetEnumerator());

        // Act
        var result = await _controller.GetBooks();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Book>>>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Book>>(actionResult.Value);
        Assert.Equal(2, model.Count());
    }
}
```

### Documentation

#### Running the Application

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd <repository-directory>
   ```

2. **Set up the database**:
   Ensure your `appsettings.json` is configured with the correct connection string for your database.

3. **Apply migrations**:
   ```bash
   dotnet ef database update
   ```

4. **Run the application**:
   ```bash
   dotnet run
   ```

#### Interacting with API Endpoints

You can interact with the API endpoints using tools like Postman or cURL. Below are some example requests.

- **Get all books**:
  ```bash
  GET /api/book
  ```

- **Get a book by Id**:
  ```bash
  GET /api/book/{Id}
  ```

- **Add a new book**:
  ```bash
  POST /api/book
  Content-Type: application/json
  {
      "title": "New Book",
      "author": "New Author",
      "publicationYear": 2024,
      "isbn": "1234567890123"
  }
  ```

- **Update a book**:
  ```bash
  PUT /api/book/{Id}
  Content-Type: application/json
  {
      "Id": 1,
      "title": "Updated Title",
      "author": "Updated Author",
      "publicationYear": 2024,
      "isbn": "1234567890123"
  }
  ```

- **Delete a book**:
  ```bash
  DELETE /api/book/{Id}
  ```


