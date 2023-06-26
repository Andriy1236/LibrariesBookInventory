using FluentValidation;
using LibrariesBookInventory.DomainServices.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace LibrariesBookInventory.Application.Book.Common
{
    public class BookCommandValidator : AbstractValidator<BookCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        public BookCommandValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            RuleFor(x => x)
                .NotNull()
                .DependentRules(() =>
                {
                    RuleFor(y => y.ISBN)
                        .NotEmpty()
                        .DependentRules(() =>
                        {
                        RuleFor(x => x.ISBN)
                            .Must(IsISBNValid)
                            .WithMessage($"{nameof(BookCommand.ISBN)} is not valid");
                        })
                        .WithMessage($"Please specify a {nameof(BookCommand.ISBN)}");
                    
                    RuleFor(y => y.Quantity)
                        .NotEmpty()
                        .DependentRules(() =>
                        {
                            RuleFor(y => y.Quantity)
                                .GreaterThan(0)
                                .WithMessage($"{nameof(BookCommand.Quantity)} should be more than 0");
                        })
                        .WithMessage($"Please specify a {nameof(BookCommand.Quantity)}");

                    RuleFor(y => y.Title)
                        .NotEmpty()
                        .DependentRules(() =>
                        {
                            RuleFor(y => y.Title)
                                .MaximumLength(100)
                                .WithMessage($"{nameof(BookCommand.Title)} is so big. Max 100 characters");
                        })
                        .WithMessage($"Please specify a {nameof(BookCommand.Title)}");

                    RuleFor(y => y.PublicationYear)
                        .NotEmpty()
                        .WithMessage($"Please specify a {nameof(BookCommand.PublicationYear)}");
                    
                    RuleFor(y => y.Author)
                        .NotEmpty()
                        .DependentRules(() =>
                        {
                            RuleFor(y => y.Author)
                                .MaximumLength(100)
                                .WithMessage($"{nameof(BookCommand.Author)} is so big. Max 100 characters");
                        })
                        .WithMessage($"Please specify an {nameof(BookCommand.Author)}");

                    RuleFor(y => y.CategoryId)
                        .NotEmpty()
                        .DependentRules(() =>
                        {
                            RuleFor(y => y.CategoryId)
                                .MustAsync(IsCategoryExist)
                                .WithMessage($"{nameof(BookCommand.CategoryId)} not exist");
                        })
                        .WithMessage($"Please specify a {nameof(BookCommand.CategoryId)}");
                })
                .WithMessage("Object is null");
        }

        private async Task<bool> IsCategoryExist(int? id, CancellationToken cancellationToken)
        {
            return id != null && await _categoryRepository.Get(id.Value, cancellationToken) != null;
        }

        private bool IsISBNValid(string isbn)
        {
            var cleanISBN = isbn.Replace("-", "").Replace(" ", "");

            if (cleanISBN.Length != 10 && cleanISBN.Length != 13)
                return false;

            return cleanISBN.Length == 10 
                    ? ValidateISBN10(cleanISBN) 
                    : ValidateISBN13(cleanISBN);
        }

        private static bool ValidateISBN10(string isbn)
        {
            var checksum = 0;

            for (var i = 0; i < 9; i++)
            {
                if (!char.IsDigit(isbn[i]))
                    return false;

                checksum += (i + 1) * (isbn[i] - '0');
            }

            if (isbn[9] == 'X')
                checksum += 10;
            else if (char.IsDigit(isbn[9]))
                checksum += (isbn[9] - '0');
            else
                return false;

            return (checksum % 11 == 0);
        }

        private static bool ValidateISBN13(string isbn)
        {
            var checksum = 0;

            for (var i = 0; i < 12; i++)
            {
                if (!char.IsDigit(isbn[i]))
                    return false;

                var digit = isbn[i] - '0';
                checksum += (i % 2 == 0) ? digit : digit * 3;
            }

            var remainder = checksum % 10;
            var checkDigit = (remainder == 0) ? 0 : 10 - remainder;

            return (checkDigit == isbn[12] - '0');
        }
    }
}