using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using SolidApiExample.Api.Auth;

namespace SolidApiExample.UnitTests.Api.Auth;

public sealed class ApiKeyAuthenticationHandlerTests
{
    private static ApiKeyAuthenticationHandler CreateHandler(string expectedKey, HttpContext context)
    {
        var options = new ApiKeyOptions { ExpectedKey = expectedKey };
        var optionsMonitor = new Mock<IOptionsMonitor<ApiKeyOptions>>();
        optionsMonitor.Setup(m => m.CurrentValue).Returns(options);
        optionsMonitor.Setup(m => m.Get(It.IsAny<string>())).Returns(options);

        var handler = new ApiKeyAuthenticationHandler(
            optionsMonitor.Object,
            NullLoggerFactory.Instance,
            UrlEncoder.Default);

        handler.InitializeAsync(
            new AuthenticationScheme(ApiKeyDefaults.Scheme, ApiKeyDefaults.Scheme, typeof(ApiKeyAuthenticationHandler)),
            context).GetAwaiter().GetResult();

        return handler;
    }

    [Fact]
    public async Task Authenticate_Fails_WhenHeaderMissing()
    {
        var context = new DefaultHttpContext();
        var handler = CreateHandler("expected-key", context);

        var result = await handler.AuthenticateAsync();

        Assert.False(result.Succeeded);
        Assert.Equal("API key header missing.", result.Failure?.Message);
    }

    [Fact]
    public async Task Authenticate_Fails_WhenKeyEmpty()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers[ApiKeyDefaults.HeaderName] = "";
        var handler = CreateHandler("expected-key", context);

        var result = await handler.AuthenticateAsync();

        Assert.False(result.Succeeded);
        Assert.Equal("Invalid API key.", result.Failure?.Message);
    }

    [Fact]
    public async Task Authenticate_Fails_WhenKeyIncorrect()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers[ApiKeyDefaults.HeaderName] = "wrong-key";
        var handler = CreateHandler("expected-key", context);

        var result = await handler.AuthenticateAsync();

        Assert.False(result.Succeeded);
        Assert.Equal("Invalid API key.", result.Failure?.Message);
    }

    [Fact]
    public async Task Authenticate_Succeeds_WhenKeyMatches()
    {
        const string expectedKey = "expected-key";
        var context = new DefaultHttpContext();
        context.Request.Headers[ApiKeyDefaults.HeaderName] = expectedKey;
        var handler = CreateHandler(expectedKey, context);

        var result = await handler.AuthenticateAsync();

        Assert.True(result.Succeeded);
        Assert.Equal(ApiKeyDefaults.Scheme, result.Ticket?.AuthenticationScheme);
        Assert.Equal("sample-user", result.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
    }
}
