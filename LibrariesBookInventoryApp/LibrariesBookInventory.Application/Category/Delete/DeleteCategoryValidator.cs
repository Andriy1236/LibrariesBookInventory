using FluentValidation;
using LibrariesBookInventory.DomainServices.Interfaces;
using LibrariesBookInventory.Application.Category.Common;
using LibrariesCategoryInventory.Application.Category.Common;

namespace LibrariesBookInventory.Application.Category.Delete
{
    public class DeleteCategoryValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryValidator(ICategoryRepository categoryRepository)
        {
            RuleFor(x => x.Id).SetValidator(new CategoryIdValidator(categoryRepository));
        }
    }
}