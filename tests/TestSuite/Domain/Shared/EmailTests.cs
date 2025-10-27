using SolidApiExample.Domain.Shared;

namespace SolidApiExample.TestSuite.Domain.Shared;

public sealed class EmailTests
{
    [Fact]
    public void Create_TrimsWhitespace()
    {
        var email = Email.Create("  user@example.com  ");

        Assert.Equal("user@example.com", email.Value);
    }

    [Fact]
    public void Create_Throws_WhenBlank()
    {
        Assert.Throws<ArgumentException>(() => Email.Create(" "));
    }

    [Fact]
    public void Create_Throws_WhenFormatInvalid()
    {
        Assert.Throws<ArgumentException>(() => Email.Create("not-an-email"));
    }

    [Fact]
    public void ToString_ReturnsUnderlyingValue()
    {
        var email = Email.Create("ada@example.com");

        Assert.Equal("ada@example.com", email.ToString());
    }
}
