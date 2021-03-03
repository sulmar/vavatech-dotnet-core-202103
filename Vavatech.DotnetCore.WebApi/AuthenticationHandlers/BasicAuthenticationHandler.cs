using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Vavatech.DotnetCore.Models;
using IServices = Vavatech.DotnetCore.IServices;

namespace Vavatech.DotnetCore.WebApi.AuthenticationHandlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const string authorizationKey = "Authorization";
        private readonly IServices.IAuthenticationService authenticationService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            IServices.IAuthenticationService authenticationService) : base(options, logger, encoder, clock)
        {
            this.authenticationService = authenticationService;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(authorizationKey))
            {
                return AuthenticateResult.Fail("Missing authorization header");
            }

            var authorizationHeader = AuthenticationHeaderValue.Parse(Request.Headers[authorizationKey]);

            if (authorizationHeader.Scheme != "Basic")
            {
                return AuthenticateResult.Fail("Invalid schema");
            }

            byte[] credentialBytes = Convert.FromBase64String(authorizationHeader.Parameter);
            string[] credentials = Encoding.UTF8.GetString(credentialBytes).Split(":");

            string login = credentials[0];
            string password = credentials[1];

            if (!authenticationService.TryAuthorize(login, password, out Customer customer))
            {
                return AuthenticateResult.Fail("Invalid login or password");
            }

            ClaimsIdentity identity = new ClaimsIdentity(Scheme.Name);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);

        }
    }
}
