using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentValidation.TestHelper;
using LibrariesBookInventory.Application.Book.Search;
using LibrariesBookInventory.Application.Book;
using LibrariesBookInventory.DomainServices.Models;

namespace LibrariesBookInventory.Tests.Validators.Book
{

    namespace LibrariesBookInventory.Application.Book.Search.Tests
    {
        [TestClass]
        public class SearchBookValidatorTests
        {
            private SearchBookValidator _validator;

            [TestInitialize]
            public void TestInitialize()
            {
                _validator = new SearchBookValidator();
            }

            [TestMethod]
            public void Validate_WhenSortColumnIsNull_ShouldNotHaveValidationError()
            {
                var query = new SearchBooksQuery { Args = new SearchBooksParameters() { SortColumn = null } };
                var result = _validator.TestValidate(query);

                result.ShouldHaveValidationErrorFor(x => x.Args.SortColumn)
                    .WithErrorMessage($"{nameof(SearchBooksQuery.Args.SortColumn)} is null");
            }

            [TestMethod]
            public void Validate_WhenSortColumnIsEmpty_ShouldNotHaveValidationError()
            {
                var query = new SearchBooksQuery { Args = new SearchBooksParameters() { SortColumn = "" } };
                var result = _validator.TestValidate(query);

                result.ShouldHaveValidationErrorFor(x => x.Args.SortColumn)
                    .WithErrorMessage($"{nameof(SearchBooksQuery.Args.SortColumn)} is not exist");
            }

            [TestMethod]
            public void Validate_WhenSortColumnDoesNotExist_ShouldHaveValidationError()
            {
                var query = new SearchBooksQuery { Args = new SearchBooksParameters() { SortColumn = "InvalidColumn" } };
                var result = _validator.TestValidate(query);

                result.ShouldHaveValidationErrorFor(x => x.Args.SortColumn)
                    .WithErrorMessage($"{nameof(SearchBooksQuery.Args.SortColumn)} is not exist");
            }

            [TestMethod]
            public void Validate_WhenPageNumberIsZero_ShouldHaveValidationError()
            {
                var query = new SearchBooksQuery { Args = new SearchBooksParameters() { PageNumber = 0 } };
                var result = _validator.TestValidate(query);

                result.ShouldHaveValidationErrorFor(x => x.Args.PageNumber)
                    .WithErrorMessage($"{nameof(SearchBooksQuery.Args.PageNumber)} should be greater than 0");
            }

            [TestMethod]
            public void Validate_WhenSortDirectionIsNotValid_ShouldHaveValidationError()
            {
                var query = new SearchBooksQuery { Args = new SearchBooksParameters() { SortDirection = "InvalidDirection" } };
                var result = _validator.TestValidate(query);

                result.ShouldHaveValidationErrorFor(x => x.Args.SortDirection)
                    .WithErrorMessage($"{nameof(SearchBooksQuery.Args.SortDirection)} should be asc or desc");
            }

            [TestMethod]
            public void Validate_WhenPageSizeIsZero_ShouldHaveValidationError()
            {
                var query = new SearchBooksQuery { Args = new SearchBooksParameters() { PageSize = 0 } };
                var result = _validator.TestValidate(query);

                result.ShouldHaveValidationErrorFor(x => x.Args.PageSize)
                    .WithErrorMessage($"{nameof(SearchBooksQuery.Args.PageSize)} should be greater than 0");
            }

            [TestMethod]
            public void Validate_WhenSearchTextIsLessThanMinimumLength_ShouldHaveValidationError()
            {
                var query = new SearchBooksQuery { Args = new SearchBooksParameters() { SearchText = "ab" } };
                var result = _validator.TestValidate(query);

                result.ShouldHaveValidationErrorFor(x => x.Args.SearchText)
                    .WithErrorMessage($"{nameof(SearchBooksQuery.Args.SearchText)} should have minimum length 3 characters");
            }
        }
    }

}
