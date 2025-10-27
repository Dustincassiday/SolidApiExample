
using SolidApiExample.Domain.Customers;

namespace SolidApiExample.TestSuite.Domain.Customers;

public sealed class CustomerTests
{
    [Fact]
    public void Create_WithEmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() => Customer.Create(" "));
    }

    [Fact]
    public void FromExisting_WithEmptyId_Throws()
    {
        Assert.Throws<ArgumentException>(() => Customer.FromExisting(Guid.Empty, "Ada"));
    }

    [Fact]
    public void Rename_UpdatesName()
    {
        var customer = Customer.Create("Ada");

        customer.Rename("Ada King");

        Assert.Equal("Ada King", customer.Name);
    }
}
