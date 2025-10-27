using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using SolidApiExample.Application.Pipeline;
using AppValidationException = SolidApiExample.Application.Validation.ValidationException;

namespace SolidApiExample.TestSuite.Application.Pipeline;

public sealed class PipelineBehaviorTests
{
    [Fact]
    public async Task ValidationBehavior_ReturnsNextResult_WhenAllValidatorsPass()
    {
        // Arrange
        var validator = new InlineValidator<string>();
        validator.RuleFor(x => x).NotNull();

        var behavior = new ValidationBehavior<string, int>(new[] { validator });
        var expected = 42;

        // Act
        var result = await behavior.Handle("request", () => Task.FromResult(expected), CancellationToken.None);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task ValidationBehavior_ThrowsValidationException_WhenAnyValidatorFails()
    {
        // Arrange
        var validator = new InlineValidator<string>();
        validator.RuleFor(x => x).NotEqual("request").WithMessage("Invalid");

        var behavior = new ValidationBehavior<string, int>(new[] { validator });

        // Act & Assert
        await Assert.ThrowsAsync<AppValidationException>(() =>
            behavior.Handle("request", () => Task.FromResult(0), CancellationToken.None));
    }

    [Fact]
    public async Task LoggingBehavior_LogsAroundNextDelegate()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<LoggingBehavior<string, int>>>();
        var behavior = new LoggingBehavior<string, int>(loggerMock.Object);
        var expected = 7;

        // Act
        var result = await behavior.Handle("request", () => Task.FromResult(expected), CancellationToken.None);

        // Assert
        Assert.Equal(expected, result);
        loggerMock.Verify(
            logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) => state.ToString()!.Contains("Handling") || state.ToString()!.Contains("Handled")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Exactly(2));
    }
}
