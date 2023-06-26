using FluentValidation;
using LibrariesBookInventory.DomainServices.Interfaces;
using System.Threading.Tasks;
using System.Threading;

namespace LibrariesBookInventory.Application.Book.Common
{
    public class BookIdValidator : AbstractValidator<long>
    {
        private readonly IBookRepository _repository;

        public BookIdValidator(IBookRepository repository)
        {
            _repository = repository;

            RuleFor(x => x)
                .MustAsync(IsExist)
                .WithMessage($"Book id is not exist")
                .WithErrorCode(ErrorsCodes.ItemNotFound);
        }
        private async Task<bool> IsExist(long id, CancellationToken cancellationToken)
        {
            return await _repository.Get(id, cancellationToken) != null;
        }
    }
}