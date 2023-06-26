using FluentValidation;
using LibrariesBookInventory.DomainServices.Interfaces;
using LibrariesBookInventory.Application.Book.Common;

namespace LibrariesBookInventory.Application.Book.Delete
{
    public class DeleteBookValidator : AbstractValidator<DeleteBookCommand>
    {
        public DeleteBookValidator(IBookRepository bookRepository)
        {
            RuleFor(x => x.Id).SetValidator(new BookIdValidator(bookRepository));
        }
    }
}