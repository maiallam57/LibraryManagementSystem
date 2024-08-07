
using LibraryManagementSystem.DataAccess;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using LibraryManagementSystem.Controllers;

namespace LibraryManagementSystem.Service
{
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

            _mockSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(books.Provider);
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
}
