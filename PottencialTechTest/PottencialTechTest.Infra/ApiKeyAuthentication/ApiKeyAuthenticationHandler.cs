using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace PottencialTechTest.Infra.ApiKeyAuthentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IConfiguration _configuration;
        private const string APIKEYNAME = "X-Api-Key";

        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
                return Task.FromResult(AuthenticateResult.Fail("API Key inválida."));

            var apiKey = _configuration["ApiKey"];

            if (!apiKey.Equals(extractedApiKey))
                return Task.FromResult(AuthenticateResult.Fail("API Key inválida."));


            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "API") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
    }
}
