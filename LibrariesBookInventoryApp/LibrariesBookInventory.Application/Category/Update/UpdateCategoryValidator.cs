using FluentValidation;
using LibrariesBookInventory.DomainServices.Interfaces;
using LibrariesBookInventory.Application.Category.Common;
using LibrariesCategoryInventory.Application.Category.Common;

namespace LibrariesBookInventory.Application.Category.Update
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator(ICategoryRepository categoryRepository)
        {
            Include(new BaseCategoryCommandValidator());
            RuleFor(x => x.Id).SetValidator(new CategoryIdValidator(categoryRepository));
        }
    }
}