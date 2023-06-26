using FluentValidation;
using LibrariesBookInventory.Application.Book.Common;
using LibrariesBookInventory.DomainServices.Interfaces;

namespace LibrariesBookInventory.Application.Book.Create
{
    public class CreateBookValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookValidator(ICategoryRepository categoryRepository)
        {
            Include(new BookCommandValidator(categoryRepository));
        }
    }
}