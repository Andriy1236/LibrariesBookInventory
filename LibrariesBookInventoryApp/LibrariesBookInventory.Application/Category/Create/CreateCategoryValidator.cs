using FluentValidation;
using LibrariesBookInventory.Application.Category.Common;

namespace LibrariesBookInventory.Application.Category.Create
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            Include(new BaseCategoryCommandValidator());    
        }
    }
}