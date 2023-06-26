using LibrariesBookInventory.DomainServices.Implementation;
using LibrariesBookInventory.Tests.Repositories.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibrariesBookInventory.Domain.Models;

namespace LibrariesBookInventory.Tests.Repositories
{
    [TestClass]
    public class BookRepositoryTests
    {
        private BookRepository _bookRepository;
        private Mock<DbContext> _dbContextMock;
        private Mock<DbSet<Book>> _dbSetMock;

        [TestInitialize]
        public void Setup()
        {
            _dbContextMock = new Mock<DbContext>();
            _dbSetMock = new Mock<DbSet<Book>>();
            _dbContextMock.Setup(c => c.Set<Book>()).Returns(_dbSetMock.Object);
            _bookRepository = new BookRepository(_dbContextMock.Object);
        }

        [TestMethod]
        public async Task Create_AddsBookToDatabase()
        {
            var book = new Book { Id = 1, Title = "Harry Potter and the Philosopher's Stone" };

            _bookRepository.Create(book);
            await _bookRepository.SaveChanges(new CancellationToken());

            _dbSetMock.Verify(mock => mock.Add(book), Times.Once);
        }

        [TestMethod]
        public async Task Delete_RemovesBookFromDatabase()
        {
            var cancellationToken = new CancellationToken();
            var book = new Book { Id = 1, Title = "Harry Potter and the Philosopher's Stone" };
            _dbSetMock.Setup(mock => mock.FindAsync(cancellationToken, book.Id)).ReturnsAsync(book);

            await _bookRepository.Delete(book.Id, cancellationToken);

            _dbSetMock.Verify(mock => mock.Remove(book), Times.Once);
        }

        [TestMethod]
        public async Task Get_ReturnsBookFromDatabase()
        {
            var cancellationToken = new CancellationToken();
            var book = new Book { Id = 1, Title = "Harry Potter and the Philosopher's Stone" };
            _dbSetMock.Setup(mock => mock.FindAsync(cancellationToken, book.Id)).ReturnsAsync(book);

            var result = await _bookRepository.Get(book.Id, cancellationToken);

            Assert.AreEqual(book, result);
        }

        [TestMethod]
        public async Task GetAll_ReturnsAllBooksFromDatabase()
        {
            IEnumerable<Book> books = new List<Book>
            {
                new Book { Id = 1, Title = "Harry Potter and the Philosopher's Stone" },
                new Book { Id = 2, Title = "Harry Potter and the Chamber of Secrets" }
            };

            var context = new Mock<DbContext>();
            var dbSetMock = new Mock<DbSet<Book>>();
            dbSetMock.As<IQueryable<Book>>().Setup(x => x.Provider).Returns(books.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Book>>().Setup(x => x.Expression).Returns(books.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Book>>().Setup(x => x.ElementType).Returns(books.AsQueryable().ElementType);
            dbSetMock.As<IDbAsyncEnumerable<Book>>().Setup(x => x.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<Book>(books.GetEnumerator()));

            context.Setup(x => x.Set<Book>()).Returns(dbSetMock.Object);

            _bookRepository = new BookRepository(context.Object);
            var result = await _bookRepository.GetAll(new CancellationToken());

            foreach (var book in books)
            {
                var resultBook = result.First(x => x.Id == book.Id);
                Assert.AreEqual(book, resultBook);
            }
        }
    }
}
