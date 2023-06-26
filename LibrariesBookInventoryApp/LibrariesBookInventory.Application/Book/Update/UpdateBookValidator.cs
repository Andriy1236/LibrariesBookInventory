using FluentValidation;
using LibrariesBookInventory.DomainServices.Interfaces;
using LibrariesBookInventory.Application.Book.Common;

namespace LibrariesBookInventory.Application.Book.Update
{
    public class UpdateBookValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookValidator(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            Include(new BookCommandValidator(categoryRepository));
            RuleFor(x => x.Id).SetValidator(new BookIdValidator(bookRepository));
        }
    }
}