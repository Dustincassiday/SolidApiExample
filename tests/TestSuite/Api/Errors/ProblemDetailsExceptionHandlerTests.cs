using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolidApiExample.Api.Errors;
using SolidApiExample.Application.Validation;

namespace SolidApiExample.TestSuite.Api.Errors;

public sealed class ProblemDetailsExceptionHandlerTests
{
    private readonly Mock<ILogger<ProblemDetailsExceptionHandler>> _loggerMock = new();
    private readonly ProblemDetailsExceptionHandler _handler;

    public ProblemDetailsExceptionHandlerTests() => _handler = new ProblemDetailsExceptionHandler(_loggerMock.Object);

    [Fact]
    public async Task MapsValidationExceptionToBadRequest()
    {
        // Arrange
        var exception = new ValidationException(new[] { "Name is required." });

        // Act
        var (context, problem, handled) = await HandleExceptionAsync(exception);

        // Assert
        Assert.True(handled);
        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        Assert.Equal("Validation failure", problem.Title);
        Assert.Equal("One or more validation errors occurred.", problem.Detail);
    }

    [Fact]
    public async Task MapsKeyNotFoundExceptionToNotFound()
    {
        // Arrange
        var exception = new KeyNotFoundException("Order not found");

        // Act
        var (context, problem, handled) = await HandleExceptionAsync(exception);

        // Assert
        Assert.True(handled);
        Assert.Equal((int)HttpStatusCode.NotFound, context.Response.StatusCode);
        Assert.Equal("Resource not found", problem.Title);
        Assert.Equal("Order not found", problem.Detail);
    }

    [Fact]
    public async Task MapsUnhandledExceptionToInternalServerError()
    {
        // Arrange
        var exception = new InvalidOperationException("Unexpected failure");

        // Act
        var (context, problem, handled) = await HandleExceptionAsync(exception);

        // Assert
        Assert.True(handled);
        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        Assert.Equal("An unexpected error occurred", problem.Title);

        _loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((_, _) => true),
            exception,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    private static DefaultHttpContext CreateHttpContext() =>
        new()
        {
            Response = { Body = new MemoryStream() }
        };

    private async Task<(HttpContext Context, ProblemDetails Problem, bool Handled)> HandleExceptionAsync(Exception exception)
    {
        var context = CreateHttpContext();
        var handled = await _handler.TryHandleAsync(context, exception, CancellationToken.None);
        context.Response.Body.Position = 0;
        var problem = await DeserializeProblemDetails(context.Response);
        return (context, problem, handled);
    }

    private static async Task<ProblemDetails> DeserializeProblemDetails(HttpResponse response)
    {
        response.Body.Position = 0;
        return (await JsonSerializer.DeserializeAsync<ProblemDetails>(
            response.Body,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }))!;
    }
}
