using FluentValidation.TestHelper;
using LibrariesBookInventory.Application;
using LibrariesBookInventory.DomainServices.Interfaces;
using LibrariesCategoryInventory.Application.Category.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading;

namespace LibrariesBookInventory.Tests.Validators.Category
{
    [TestClass]
    public class CategoryIdValidatorTests
    {
        private CategoryIdValidator _validator;
        private Mock<ICategoryRepository> _repositoryMock;

        [TestInitialize]
        public void Setup()
        {
            _repositoryMock = new Mock<ICategoryRepository>();
            _validator = new CategoryIdValidator(_repositoryMock.Object);
        }

        [TestMethod]
        public void Validate_InvalidCategoryId_ShouldHaveValidationError()
        {
            var categoryId = 1;
            _repositoryMock.Setup(x => x.Get(categoryId, CancellationToken.None))
                          .ReturnsAsync((Domain.Models.Category)null);

            var result = _validator.Validate(categoryId);

            var hasErrors = result.Errors
                  .WithErrorMessage("Category id is not exist")
                  .WithErrorCode(ErrorsCodes.ItemNotFound);

            Assert.IsTrue(hasErrors.Any());
        }

        [TestMethod]
        public void Validate_ValidCategoryId_ShouldNotHaveValidationError()
        {
            var categoryId = 1;
            _repositoryMock.Setup(x => x.Get(categoryId, CancellationToken.None))
                          .ReturnsAsync(new Domain.Models.Category());

            var result = _validator.Validate(categoryId);

            Assert.IsTrue(result.IsValid);
        }
    }
}