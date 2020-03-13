using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Visit.Service.Models.Responses
{
    public abstract class BaseGatewayResponse
    {
        public bool Success { get; }
        public IEnumerable<IdentityError> Errors { get; }

        protected BaseGatewayResponse(bool success=false, IEnumerable<IdentityError> errors=null)
        {
            Success = success;
            Errors = errors;
        }
    }
}