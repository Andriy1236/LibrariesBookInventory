using FluentValidation;
using LibrariesBookInventory.DomainServices.Interfaces;
using LibrariesBookInventory.Application.Category.Common;
using LibrariesCategoryInventory.Application.Category.Common;

namespace LibrariesBookInventory.Application.Category.Get
{
    internal class GetCategoryValidator : AbstractValidator<GetCategoryQuery>
    {
        public GetCategoryValidator(ICategoryRepository categoryRepository)
        {
            RuleFor(x => x.Id).SetValidator(new CategoryIdValidator(categoryRepository));
        }
    }
}