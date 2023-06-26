using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace LibrariesBookInventory.Application
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {

        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validatorTasks = _validators
                .Select(v => v.ValidateAsync(context, cancellationToken))
                .ToList();

            await Task.WhenAll(validatorTasks);

            var errors = validatorTasks
                .SelectMany(t => t.Result.Errors)
                .Where(f => f != null)
                .ToList();

            if (errors.Count != 0)
            {
                return (TResponse)Activator.CreateInstance(typeof(TResponse), errors);
            }

            return await next();
        }
    }
}