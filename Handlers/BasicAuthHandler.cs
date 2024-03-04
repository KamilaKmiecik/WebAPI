using System;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using WebApplicationYes.Attributes;
using WebApplicationYes.Models.AdditionalClasses;

namespace WebApplicationYes.Handler
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var key = "Authorization";

            if (!Request.Headers.ContainsKey(key))
                return AuthenticateResult.Fail("Brak klucza");

            var authorizationHeader = Request.Headers[key].ToString();

            if (!authorizationHeader.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.Fail("Klucz musi zaczynać się 'Basic'");

            var authBaseDecoded = Encoding.UTF8.GetString(
                Convert.FromBase64String(authorizationHeader.Replace("Basic", "", StringComparison.OrdinalIgnoreCase)
                ));

            var authSplit = authBaseDecoded.Split(new[] { ':' }, 2);

            if (authSplit.Length != 2)
                return AuthenticateResult.Fail("Invalid format'");

            var login = authSplit[0];
            var password = authSplit[1];

            if (!IsValidCredentials(login, password))
                return AuthenticateResult.Fail("Nieprawidłowy login/hasło");

            var client = new BasicAuthClient()
            {
                AuthenticationType = BasicAuthenticationDefaults.AuthenticationScheme,
                IsAuthenticated = true,
                Name = login
            };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(client, new[]
             {
                new Claim(ClaimTypes.Name, login)
            }));

            Context.User = claimsPrincipal;

            return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
        }

        private bool IsValidCredentials(string login, string password)
        {

            return login == EnovaConfig.APIKey; 
        }

    }
}
