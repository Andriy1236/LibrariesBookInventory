using FluentValidation;

namespace LibrariesBookInventory.Application.Category.Common
{
    public class BaseCategoryCommandValidator : AbstractValidator<BaseCategoryCommand>
    {
        public BaseCategoryCommandValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .DependentRules(() =>
                {
                    RuleFor(x => x.Name)
                        .NotEmpty()
                        .WithMessage($"Please specify a {nameof(BaseCategoryCommand.Name)}");

                    RuleFor(x => x.Name)
                        .MaximumLength(100)
                        .WithMessage($"{nameof(BaseCategoryCommand.Name)} is too big. Max length is 100");

                    RuleFor(x => x.Description)
                        .NotEmpty()
                        .WithMessage($"Please specify a {nameof(BaseCategoryCommand.Description)}");

                    RuleFor(x => x.Description)
                        .MaximumLength(100)
                        .WithMessage($"{nameof(BaseCategoryCommand.Description)} is too big. Max length is 100");
                })
                .WithMessage("Object is null");
        }
    }
}