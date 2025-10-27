using SolidApiExample.Domain.Shared;

namespace SolidApiExample.TestSuite.Domain.Shared;

public sealed class MoneyTests
{
    [Fact]
    public void Create_NormalizesCurrency()
    {
        var money = Money.Create(10m, "usd");

        Assert.Equal("USD", money.Currency);
    }

    [Fact]
    public void Create_RoundsAmountToTwoDecimals()
    {
        var money = Money.Create(10.555m, "USD");

        Assert.Equal(10.56m, money.Amount);
    }

    [Fact]
    public void Create_Throws_WhenAmountNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Money.Create(-0.01m, "USD"));
    }

    [Fact]
    public void Create_Throws_WhenCurrencyMissing()
    {
        Assert.Throws<ArgumentException>(() => Money.Create(10m, " "));
    }

    [Fact]
    public void Create_Throws_WhenCurrencyNotThreeLetters()
    {
        Assert.Throws<ArgumentException>(() => Money.Create(10m, "US"));
    }

    [Fact]
    public void Add_ReturnsSum_WhenSameCurrency()
    {
        var left = Money.Create(5m, "USD");
        var right = Money.Create(3m, "USD");

        var sum = left.Add(right);

        Assert.Equal(8m, sum.Amount);
        Assert.Equal("USD", sum.Currency);
    }

    [Fact]
    public void Add_Throws_WhenCurrencyDiffers()
    {
        var left = Money.Create(5m, "USD");
        var right = Money.Create(3m, "EUR");

        Assert.Throws<InvalidOperationException>(() => left.Add(right));
    }

    [Fact]
    public void Subtract_ReturnsDifference_WhenEnoughAmount()
    {
        var left = Money.Create(5m, "USD");
        var right = Money.Create(2m, "USD");

        var result = left.Subtract(right);

        Assert.Equal(3m, result.Amount);
    }

    [Fact]
    public void Subtract_Throws_WhenResultNegative()
    {
        var left = Money.Create(2m, "USD");
        var right = Money.Create(5m, "USD");

        Assert.Throws<InvalidOperationException>(() => left.Subtract(right));
    }

    [Fact]
    public void Subtract_Throws_WhenCurrencyDiffers()
    {
        var left = Money.Create(5m, "USD");
        var right = Money.Create(3m, "EUR");

        Assert.Throws<InvalidOperationException>(() => left.Subtract(right));
    }
}
