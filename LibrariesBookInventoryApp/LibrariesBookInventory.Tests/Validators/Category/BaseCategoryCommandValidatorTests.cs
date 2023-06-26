using FluentValidation.TestHelper;
using LibrariesBookInventory.Application.Category.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibrariesBookInventory.Tests.Validators.Category
{
    [TestClass]
    public class BaseCategoryCommandValidatorTests
    {
        private BaseCategoryCommandValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new BaseCategoryCommandValidator();
        }

        [TestMethod]
        public void Validate_MissingName_ShouldHaveValidationError()
        {
            var command = new BaseCategoryCommand
            {
                Description = "Sample description"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Please specify a Name");
        }

        [TestMethod]
        public void Validate_LongName_ShouldHaveValidationError()
        {
            var command = new BaseCategoryCommand
            {
                Name = new string('A', 101),
                Description = "Sample description"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Name is too big. Max length is 100");
        }

        [TestMethod]
        public void Validate_MissingDescription_ShouldHaveValidationError()
        {

            var command = new BaseCategoryCommand
            {
                Name = "Sample name"
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage("Please specify a Description");
        }

        [TestMethod]
        public void Validate_LongDescription_ShouldHaveValidationError()
        {
            var command = new BaseCategoryCommand
            {
                Name = "Sample name",
                Description = new string('A', 101)
            };

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage("Description is too big. Max length is 100");
        }

        [TestMethod]
        public void Validate_ValidCommand_ShouldNotHaveValidationError()
        {
            var command = new BaseCategoryCommand
            {
                Name = "Sample name",
                Description = "Sample description"
            };

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
