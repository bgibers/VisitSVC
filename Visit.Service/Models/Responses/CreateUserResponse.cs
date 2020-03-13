using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Visit.DataAccess.Auth;

namespace Visit.Service.Models.Responses
{
    public class CreateUserResponse : BaseGatewayResponse
    {
        public JwtToken JwtToken { get; set; }
        
        public CreateUserResponse(JwtToken jwtToken, bool success = false, IEnumerable<IdentityError> errors = null) : base(success, errors)
        {
            JwtToken = jwtToken;
        }
    }
}