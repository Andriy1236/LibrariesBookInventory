using FluentValidation;
using LibrariesBookInventory.Application.Book.Common;
using LibrariesBookInventory.DomainServices.Interfaces;

namespace LibrariesBookInventory.Application.Book.Get
{
    public class GetBookValidator : AbstractValidator<GetBookQuery>
    {
        public GetBookValidator(IBookRepository bookRepository)
        {
            RuleFor(x => x.Id).SetValidator(new BookIdValidator(bookRepository));
        }
    }
}