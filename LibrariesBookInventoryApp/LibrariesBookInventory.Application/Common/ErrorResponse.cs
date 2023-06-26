using FluentValidation.Results;
using System.Collections.Generic;

namespace LibrariesBookInventory.Application.Common
{
    public class EmptyResponse : BaseResponse<object>
    {
        public EmptyResponse(IEnumerable<ValidationFailure> validationFailures = null) : base(validationFailures)
        {
            
        }
    }
}
