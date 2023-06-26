using LibrariesBookInventory.Application.Book.Common;
using LibrariesBookInventory.Application;
using LibrariesBookInventory.DomainServices.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading;
using FluentValidation.TestHelper;

namespace LibrariesBookInventory.Tests.Validators.Book
{
    [TestClass]
    public class BookIdValidatorTests
    {
        private BookIdValidator _validator;
        private Mock<IBookRepository> _repositoryMock;

        [TestInitialize]
        public void Setup()
        {
            _repositoryMock = new Mock<IBookRepository>();
            _validator = new BookIdValidator(_repositoryMock.Object);
        }

        [TestMethod]
        public void Validate_InvalidBookId_ShouldHaveValidationError()
        {
            long bookId = 1;
            _repositoryMock.Setup(x => x.Get(bookId, CancellationToken.None))
                          .ReturnsAsync((Domain.Models.Book)null);

            var result = _validator.Validate(bookId);

            var hasErrors = result.Errors
                  .WithErrorMessage("Book id is not exist")
                  .WithErrorCode(ErrorsCodes.ItemNotFound);

            Assert.IsTrue(hasErrors.Any());
        }

        [TestMethod]
        public void Validate_ValidBookId_ShouldNotHaveValidationError()
        {
            long bookId = 1;
            _repositoryMock.Setup(x => x.Get(bookId, CancellationToken.None))
                          .ReturnsAsync(new Domain.Models.Book());

            var result = _validator.Validate(bookId);

            Assert.IsTrue(result.IsValid);
        }
    }
}
