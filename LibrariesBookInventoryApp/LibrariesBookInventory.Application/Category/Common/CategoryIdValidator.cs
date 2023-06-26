using FluentValidation;
using System.Threading.Tasks;
using System.Threading;
using System;
using LibrariesBookInventory.DomainServices.Interfaces;
using LibrariesBookInventory.Application;

namespace LibrariesCategoryInventory.Application.Category.Common
{
    public class CategoryIdValidator : AbstractValidator<int>
    {
        private readonly ICategoryRepository _repository;

        public CategoryIdValidator(ICategoryRepository repository)
        {
            _repository = repository;

            RuleFor(x => x)
                .MustAsync(IsExist)
                .WithMessage($"Category id is not exist")
                .WithErrorCode(ErrorsCodes.ItemNotFound);
        }
        private async Task<bool> IsExist(int id, CancellationToken cancellationToken)
        {
            return await _repository.Get(id, cancellationToken) != null;
        }
    }
}