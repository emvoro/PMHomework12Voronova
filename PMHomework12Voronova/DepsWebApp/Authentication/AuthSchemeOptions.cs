using Microsoft.AspNetCore.Authentication;

namespace DepsWebApp.Authentication
{
    public class AuthSchemeOptions : AuthenticationSchemeOptions
    {
        public AuthSchemeOptions()
        {
            ClaimsIssuer = AuthScheme.Issuer;
        }
    }
}
