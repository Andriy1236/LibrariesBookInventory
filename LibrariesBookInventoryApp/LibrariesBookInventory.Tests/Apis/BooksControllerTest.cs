using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LibrariesBookInventory.Application.Book;
using LibrariesBookInventory.Application.Book.Common;
using LibrariesBookInventory.Application.Book.Create;
using LibrariesBookInventory.Application.Book.Update;
using LibrariesBookInventory.Domain.Models;
using LibrariesBookInventory.Tests.Apis.Helpers;
using LibrariesBookInventory.DomainServices.Models;

namespace LibrariesBookInventory.Tests.Apis
{
    [TestClass]
    public class BooksControllerTest : BaseControllerTest
    {
        protected override string BaseApi { get; } = "api/Books";

        [TestMethod]
        public async Task Get_Book_By_Not_Exist_Id_Error()
        {
            var bookId = await GetMaxId() + 1;
            var endPoint = $"/{bookId}";

            var response = await Get(endPoint);

            var content = await response.Content.ReadAsStringAsync();

            var error = JsonConvert.DeserializeObject<string>(content);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            Assert.IsTrue(error == "Book id is not exist");
        }

        [TestMethod]
        public async Task Get_Book_By_Id_Success()
        {
            var category = await CreateValidCategory();

            var book = new Book()
            {
                Title = Guid.NewGuid().ToString().Substring(0, 17),
                Author = Guid.NewGuid().ToString().Substring(0, 17),
                ISBN = "978-3-16-148410-0",
                PublicationYear = 1996,
                Quantity = 2,
                CategoryId = category.Id
            };

            Db.Books.Add(book);
            await Db.SaveChangesAsync();

            var endPoint = $"/{book.Id}";

            var response = await Get(endPoint);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var responseBook = JsonConvert.DeserializeObject<BookDto>(content);

            Assert.IsNotNull(responseBook);
            Assert.AreEqual(responseBook.Id, book.Id);
            Assert.AreEqual(responseBook.Title, book.Title);
            Assert.AreEqual(responseBook.Author, book.Author);
            Assert.AreEqual(responseBook.PublicationYear, book.PublicationYear);
            Assert.AreEqual(responseBook.CategoryId, book.CategoryId);
            Assert.AreEqual(responseBook.ISBN, book.ISBN);
            Assert.AreEqual(responseBook.Quantity, book.Quantity);
        }

        [TestMethod]
        [DataRow("Id", "Asc")]
        [DataRow("Title", "Asc")]
        [DataRow("ISBN", "Asc")]
        [DataRow("PublicationYear", "Asc")]
        [DataRow("Quantity", "Asc")]
        [DataRow("Id", "Desc")]
        [DataRow("Title", "Desc")]
        [DataRow("ISBN", "Desc")]
        [DataRow("PublicationYear", "Desc")]
        [DataRow("Quantity", "Desc")]
        public async Task Search_Books_By_Criteria_Success(string sortColumn, string sortDirection)
        {
            var searchText = Guid.NewGuid().ToString().Substring(0, 17);
            var text = "aaa";
            var text2 = "bbb";
            var category = await CreateValidCategory();
            var book1 = new Book(searchText, text, text2, 1900, 1, category.Id);
            var book2 = new Book(text, searchText, text, 1902, 2, category.Id);
            var book3 = new Book(text2, text, searchText, 1903, 3, category.Id);

            Db.Books.Add(book1);
            Db.Books.Add(book2);
            Db.Books.Add(book3);

            await Db.SaveChangesAsync();

            var searchBooksQuery = new SearchBooksParameters()
            {
                PageNumber = 1,
                PageSize = 2,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                SearchText = searchText,
            };

            var endPoint = $"?{QueryStringConverter.ToQueryString(searchBooksQuery)}";

            var response = await Get(endPoint);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();

            var books = JsonConvert.DeserializeObject<IEnumerable<BookDto>>(content).ToList();

            var offset = (searchBooksQuery.PageNumber - 1) * searchBooksQuery.PageSize;

            var querySearchText = $"'{searchText}'";

            var query = string.Format(
                "SELECT * FROM Books WHERE Title LIKE {0} OR Author LIKE {0} OR ISBN LIKE {0} ORDER BY {1} {2} OFFSET {3} ROWS FETCH NEXT {4} ROWS ONLY",
                querySearchText, sortColumn, sortDirection, offset, searchBooksQuery.PageSize);

            var expectedBooks = await Db.Books
                .SqlQuery(query)
                .AsNoTracking()
                .ToListAsync();

            for (var i = 0; i < expectedBooks.Count; i++)
            {
                Assert.AreEqual(expectedBooks[i].Id, books[i].Id);
            }
        }

        [TestMethod]
        public async Task Search_Books_By_Invalid_Criteria_Error()
        {
            var searchBooksQuery = new SearchBooksQuery();

            var endPoint = $"?{QueryStringConverter.ToQueryString(searchBooksQuery)}";

            var response = await Get(endPoint);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();

            var errors = JsonConvert.DeserializeObject<List<string>>(content);

            Assert.IsTrue(errors.Contains("SortDirection should be asc or desc"));
            Assert.IsTrue(errors.Contains("SortColumn is null"));
            Assert.IsTrue(errors.Contains("SearchText should have minimum length 3 characters"));
        }

        [TestMethod]
        public async Task Create_Invalid_Book_Error()
        {
            var endPoint = "";

            var createBookCommand = new CreateBookCommand();

            var response = await Post(endPoint, createBookCommand);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<string[]>(content);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsTrue(errors.Contains("Please specify a Title"));
            Assert.IsTrue(errors.Contains("Please specify an Author"));
            Assert.IsTrue(errors.Contains("Please specify a ISBN"));
            Assert.IsTrue(errors.Contains("Please specify a PublicationYear"));
            Assert.IsTrue(errors.Contains("Please specify a Quantity"));
            Assert.IsTrue(errors.Contains("Please specify a CategoryId"));
        }

        [TestMethod]
        public async Task Create_Book_Success()
        {
            var createBookCommand = await GetValidCreateBookCommand();

            var endPoint = "";

            var response = await Post(endPoint, createBookCommand);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var id = JsonConvert.DeserializeObject<long>(content);

            var bookFromDb = await Db.Books.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            Assert.IsNotNull(bookFromDb);
            Assert.AreEqual(createBookCommand.Title, bookFromDb.Title);
            Assert.AreEqual(createBookCommand.Author, bookFromDb.Author);
            Assert.AreEqual(createBookCommand.PublicationYear, bookFromDb.PublicationYear);
            Assert.AreEqual(createBookCommand.Quantity, bookFromDb.Quantity);
            Assert.AreEqual(createBookCommand.CategoryId, bookFromDb.CategoryId);
        }

        [TestMethod]
        public async Task Update_Not_Exist_Book_Error()
        {
            var endPoint = $"/{await GetMaxId() + 1}";

            var updateBookCommand = new UpdateBookCommand()
            {
                Title = Guid.NewGuid().ToString().Substring(0, 17),
                Author = Guid.NewGuid().ToString().Substring(0, 17),
                ISBN = "978-3-16-148410-0",
                PublicationYear = 1996,
                Quantity = 2,
                CategoryId = 1
            };

            var response = await Put(endPoint, updateBookCommand);

            var content = await response.Content.ReadAsStringAsync();

            var error = JsonConvert.DeserializeObject<string>(content);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            Assert.IsTrue(error == "Book id is not exist");
        }

        [TestMethod]
        public async Task Update_Book_Success()
        {
            var category = await CreateValidCategory();

            var bookFromDb = new Book()
            {
                Title = Guid.NewGuid().ToString().Substring(0, 17),
                Author = Guid.NewGuid().ToString().Substring(0, 17),
                ISBN = "978-3-16-148410-0",
                PublicationYear = 1996,
                Quantity = 2,
                CategoryId = category.Id
            };

            Db.Books.Add(bookFromDb);
            await Db.SaveChangesAsync();

            var endPoint = $"/{bookFromDb.Id}";

            var updateBookCommand = new UpdateBookCommand()
            {
                Title = Guid.NewGuid().ToString().Substring(0, 17),
                Author = Guid.NewGuid().ToString().Substring(0, 17),
                ISBN = "978-3-16-148410-0",
                PublicationYear = 1997,
                Quantity = 3,
                CategoryId = category.Id
            };

            var response = await Put(endPoint, updateBookCommand);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            bookFromDb = await Db.Books.AsNoTracking().FirstOrDefaultAsync(x => x.Id == bookFromDb.Id);

            Assert.IsNotNull(bookFromDb);
            Assert.AreEqual(updateBookCommand.Title, bookFromDb.Title);
            Assert.AreEqual(updateBookCommand.Author, bookFromDb.Author);
            Assert.AreEqual(updateBookCommand.PublicationYear, bookFromDb.PublicationYear);
            Assert.AreEqual(updateBookCommand.Quantity, bookFromDb.Quantity);
            Assert.AreEqual(updateBookCommand.CategoryId, bookFromDb.CategoryId);
        }

        [TestMethod]
        public async Task Delete_Not_Exist_Book_Error()
        {
            var endPoint = $"/{await GetMaxId() + 1}";

            var response = await Delete(endPoint);

            var content = await response.Content.ReadAsStringAsync();

            var error = JsonConvert.DeserializeObject<string>(content);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            Assert.IsTrue(error == "Book id is not exist");
        }

        [TestMethod]
        public async Task Delete_Book_Success()
        {
            var category = await CreateValidCategory();

            var book = new Book()
            {
                Title = Guid.NewGuid().ToString().Substring(0, 17),
                Author = Guid.NewGuid().ToString().Substring(0, 17),
                ISBN = "978-3-16-148410-0",
                PublicationYear = 1996,
                Quantity = 2,
                CategoryId = category.Id
            };

            Db.Books.Add(book);
            await Db.SaveChangesAsync();

            var endPoint = $"/{book.Id}";

            var response = await Delete(endPoint);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var bookFromDb = await Db.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == book.Id);

            Assert.IsNull(bookFromDb);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            var books = await Db.Books.ToListAsync();
            Db.Books.RemoveRange(books);

            await Db.SaveChangesAsync();
        }

        private async Task<Category> CreateValidCategory()
        {
            var category = new Category()
            {
                Name = Guid.NewGuid().ToString().Substring(0, 17)
            };

            Db.Categories.Add(category);
            await Db.SaveChangesAsync();

            return category;
        }

        private async Task<CreateBookCommand> GetValidCreateBookCommand()
        {
            var category = await CreateValidCategory();

            return new CreateBookCommand()
            {
                Title = Guid.NewGuid().ToString().Substring(0, 17),
                Author = Guid.NewGuid().ToString().Substring(0, 17),
                ISBN = "978-3-16-148410-0",
                PublicationYear = 1996,
                Quantity = 2,
                CategoryId = category.Id
            };
        }

        private async Task<long> GetMaxId()
        {
            var maxId = await Db.Books.MaxAsync(x => (long?)x.Id);

            return maxId ?? 0;
        }

    }
}
