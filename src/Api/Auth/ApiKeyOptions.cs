using Microsoft.AspNetCore.Authentication;

namespace SolidApiExample.Api.Auth;

internal sealed class ApiKeyOptions : AuthenticationSchemeOptions
{
    public string ExpectedKey { get; set; } = string.Empty;
}
