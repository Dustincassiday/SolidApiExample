using SolidApiExample.Application.Shared;
using SolidApiExample.Application.Validation;
using SolidApiExample.Domain.Shared;

namespace SolidApiExample.TestSuite.Application.Shared;

public sealed class MoneyMappingsTests
{
    [Fact]
    public void ToDomain_TransformsDto()
    {
        var dto = new MoneyDto { Amount = 12.34m, Currency = "usd" };

        var money = dto.ToDomain();

        Assert.Equal(12.34m, money.Amount);
        Assert.Equal("USD", money.Currency);
    }

    [Fact]
    public void ToDomain_RoundsAmount()
    {
        var dto = new MoneyDto { Amount = 10.555m, Currency = "usd" };

        var money = dto.ToDomain();

        Assert.Equal(10.56m, money.Amount);
    }

    [Fact]
    public void ToDomain_ThrowsValidationException_WhenDtoNull()
    {
        var ex = Assert.Throws<ValidationException>(() => ((MoneyDto?)null)!.ToDomain());

        Assert.Contains(ex.Errors, error => error.Contains("Total must be provided."));
    }

    [Fact]
    public void ToDomain_ThrowsValidationException_WhenAmountNegative()
    {
        var dto = new MoneyDto { Amount = -1m, Currency = "USD" };

        var ex = Assert.Throws<ValidationException>(() => dto.ToDomain());

        Assert.Contains(ex.Errors, error => error.Contains("Amount cannot be negative."));
    }

    [Fact]
    public void ToDomain_ThrowsValidationException_WhenCurrencyInvalid()
    {
        var dto = new MoneyDto { Amount = 1m, Currency = "US" };

        var ex = Assert.Throws<ValidationException>(() => dto.ToDomain());

        Assert.Contains(ex.Errors, error => error.Contains("Currency must be a three-letter code."));
    }

    [Fact]
    public void ToDomain_ThrowsValidationException_WhenCurrencyMissing()
    {
        var dto = new MoneyDto { Amount = 1m, Currency = "" };

        var ex = Assert.Throws<ValidationException>(() => dto.ToDomain());

        Assert.Contains(ex.Errors, error => error.Contains("Currency must be provided."));
    }

    [Fact]
    public void ToDto_TransformsMoney()
    {
        var money = Money.Create(5.50m, "GBP");

        var dto = money.ToDto();

        Assert.Equal(5.50m, dto.Amount);
        Assert.Equal("GBP", dto.Currency);
    }
}
