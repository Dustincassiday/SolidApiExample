using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SolidApiExample.Api.Auth;

internal sealed class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyOptions>
{
    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyDefaults.HeaderName, out var values))
        {
            return Task.FromResult(AuthenticateResult.Fail("API key header missing."));
        }

        var providedKey = values.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(providedKey) || providedKey != Options.ExpectedKey)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "sample-user"),
            new Claim(ClaimTypes.Name, "Sample User")
        };

        var identity = new ClaimsIdentity(claims, ApiKeyDefaults.Scheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, ApiKeyDefaults.Scheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
