using SolidApiExample.Domain.Customers;
using SolidApiExample.Domain.Shared;

namespace SolidApiExample.TestSuite.Domain.Customers;

public sealed class CustomerTests
{
    [Fact]
    public void Create_WithEmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() => Customer.Create(" ", Email.Create("ada@example.com")));
    }

    [Fact]
    public void Create_WithNullEmail_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => Customer.Create("Ada", null!));
    }

    [Fact]
    public void FromExisting_WithEmptyId_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            Customer.FromExisting(Guid.Empty, "Ada", Email.Create("ada@example.com")));
    }

    [Fact]
    public void Rename_UpdatesName()
    {
        var customer = Customer.Create("Ada", Email.Create("ada@example.com"));

        customer.Rename("Ada King");

        Assert.Equal("Ada King", customer.Name);
    }

    [Fact]
    public void UpdateEmail_ChangesEmail()
    {
        var customer = Customer.Create("Ada", Email.Create("ada@example.com"));
        var newEmail = Email.Create("ada.king@example.com");

        customer.UpdateEmail(newEmail);

        Assert.Equal(newEmail, customer.Email);
    }
}
