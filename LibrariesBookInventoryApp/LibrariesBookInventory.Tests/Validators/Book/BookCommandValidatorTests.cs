using System.Threading;
using FluentValidation.TestHelper;
using LibrariesBookInventory.Application.Book.Common;
using LibrariesBookInventory.DomainServices.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LibrariesBookInventory.Tests.Validators.Book
{
    [TestClass]
    public class BookCommandValidatorTests
    {
        private BookCommandValidator _validator;
        private Mock<ICategoryRepository> _categoryRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _validator = new BookCommandValidator(_categoryRepositoryMock.Object);
        }

        [TestMethod]
        public void Validate_CategoryIdIsEmpty_ShouldHaveValidationError()
        {
            var command = new BookCommand { CategoryId = null };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.CategoryId)
                .WithErrorMessage("Please specify a CategoryId");
        }

        [TestMethod]
        public void Validate_InvalidCategoryId_ShouldHaveValidationError()
        {
            var categoryId = 1;
            _categoryRepositoryMock.Setup(x => x.Get(categoryId, CancellationToken.None))
                .ReturnsAsync((Domain.Models.Category)null);

            var command = new BookCommand { CategoryId = categoryId };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.CategoryId)
                .WithErrorMessage("CategoryId not exist");
        }

        [TestMethod]
        public void Validate_ISBNIsEmpty_ShouldHaveValidationError()
        {
            var command = new BookCommand { ISBN = string.Empty };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ISBN)
                .WithErrorMessage("Please specify a ISBN");
        }

        [TestMethod]
        public void Validate_InvalidISBN_ShouldHaveValidationError()
        {
            var command = new BookCommand { ISBN = "12345" };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ISBN)
                .WithErrorMessage("ISBN is not valid");
        }

        [TestMethod]
        public void Validate_QuantityIsEmpty_ShouldHaveValidationError()
        {
            var command = new BookCommand { Quantity = null };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Quantity)
                .WithErrorMessage("Please specify a Quantity");
        }

        [TestMethod]
        public void Validate_InvalidQuantity_ShouldHaveValidationError()
        {
            var command = new BookCommand { Quantity = 0 };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Quantity)
                .WithErrorMessage("Quantity should be more than 0");
        }

        [TestMethod]
        public void Validate_TitleIsEmpty_ShouldHaveValidationError()
        {
            var command = new BookCommand { Title = string.Empty };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("Please specify a Title");
        }

        [TestMethod]
        public void Validate_TitleIsTooBig_ShouldHaveValidationError()
        {
            var command = new BookCommand { Title = new string('A', 101) };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("Title is so big. Max 100 characters");
        }

        [TestMethod]
        public void Validate_PublicationYearIsEmpty_ShouldHaveValidationError()
        {
            var command = new BookCommand { PublicationYear = null };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.PublicationYear)
                .WithErrorMessage("Please specify a PublicationYear");
        }

        [TestMethod]
        public void Validate_AuthorIsEmpty_ShouldHaveValidationError()
        {
            var command = new BookCommand { Author = null };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Author)
                .WithErrorMessage("Please specify an Author");
        }

        [TestMethod]
        public void Validate_AuthorIsTooBig_ShouldHaveValidationError()
        {
            var command = new BookCommand { Author = new string('A', 101) };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Author)
                .WithErrorMessage("Author is so big. Max 100 characters");
        }
    }
}