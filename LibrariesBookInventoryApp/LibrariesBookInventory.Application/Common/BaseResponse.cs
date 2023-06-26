using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace LibrariesBookInventory.Application.Common
{
    public class BaseResponse<T>
    {
        public T Data { get; set; }
        public IEnumerable<ValidationFailure> Errors { get; }
        public bool IsOk
        {
            get
            {
                return !(Errors != null && Errors.Any());
            }
        }

        public BaseResponse(IEnumerable<ValidationFailure> validationFailures = null) 
        {
            Errors = validationFailures;
        }
    }
}