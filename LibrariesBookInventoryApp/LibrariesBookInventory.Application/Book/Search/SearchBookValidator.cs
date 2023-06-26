using System;
using System.Linq;
using FluentValidation;
using AutoMapper.Internal;

namespace LibrariesBookInventory.Application.Book.Search
{
    public class SearchBookValidator : AbstractValidator<SearchBooksQuery>
    {
        public SearchBookValidator()
        {
            RuleFor(x => x.Args.SortColumn)
                .NotNull()
                .DependentRules(() =>
                {
                    RuleFor(x => x.Args.SortColumn)
                        .Must(IsColumnExist)
                        .WithMessage($"{nameof(SearchBooksQuery.Args.SortColumn)} is not exist");
                })
                .WithMessage($"{nameof(SearchBooksQuery.Args.SortColumn)} is null");

            RuleFor(x => x.Args.PageNumber)
                .GreaterThan(0)
                .WithMessage($"{nameof(SearchBooksQuery.Args.PageNumber)} should be greater than 0");

            RuleFor(x => x.Args.SortDirection)
                .NotEmpty()
                .DependentRules(() =>
                {
                    RuleFor(x => x.Args.SortDirection)
                        .Must(IsSortingDirectionValid)
                        .WithMessage($"{nameof(SearchBooksQuery.Args.SortDirection)} should be asc or desc");
                })
                .WithMessage($"{nameof(SearchBooksQuery.Args.SortDirection)} should be asc or desc");

            RuleFor(x => x.Args.PageSize)
                .GreaterThan(0)
                .WithMessage($"{nameof(SearchBooksQuery.Args.PageSize)} should be greater than 0");

            RuleFor(x => x.Args.SearchText)
                .NotNull()
                .DependentRules(() =>
                {
                    RuleFor(x => x.Args.SearchText)
                        .MinimumLength(3)
                        .WithMessage($"{nameof(SearchBooksQuery.Args.SearchText)} should have minimum length 3 characters");

                })
                .WithMessage($"{nameof(SearchBooksQuery.Args.SearchText)} should have minimum length 3 characters");
        }

        private static bool IsSortingDirectionValid(string sortDirection)
        {
            return sortDirection.ToUpper() == "ASC" || sortDirection.ToUpper() == "DESC";
        }

        private static bool IsColumnExist(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                return false;

            var bookProperties = typeof(Domain.Models.Book).GetProperties();

            return bookProperties.Any(property =>
                property.IsPublic() && 
                property.Name != nameof(Domain.Models.Book.CategoryId) &&
                string.Equals(property.Name, columnName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}