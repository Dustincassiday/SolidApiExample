using SolidApiExample.Domain.Orders;
using SolidApiExample.Domain.Shared;


namespace SolidApiExample.TestSuite.Domain.Orders;

public sealed class OrderTests
{
    [Fact]
    public void Create_WithEmptyCustomerId_Throws()
    {
        var total = Money.Create(10m, "USD");
        Assert.Throws<ArgumentException>(() => Order.Create(Guid.Empty, total));
    }

    [Fact]
    public void Create_WithNullTotal_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => Order.Create(Guid.NewGuid(), null!));
    }

    [Fact]
    public void Create_SetsInitialStatusToNew()
    {
        var order = Order.Create(Guid.NewGuid(), Money.Create(10m, "USD"));

        Assert.Equal(OrderStatus.New, order.Status);
    }

    [Fact]
    public void FromExisting_WithEmptyId_Throws()
    {
        var total = Money.Create(10m, "USD");
        Assert.Throws<ArgumentException>(() =>
            Order.FromExisting(Guid.Empty, Guid.NewGuid(), OrderStatus.New, total));
    }

    [Fact]
    public void FromExisting_WithNullTotal_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            Order.FromExisting(Guid.NewGuid(), Guid.NewGuid(), OrderStatus.New, null!));
    }

    [Fact]
    public void UpdateStatus_AllowsAdvancingToPaid()
    {
        var order = Order.Create(Guid.NewGuid(), Money.Create(10m, "USD"));

        order.UpdateStatus(OrderStatus.Paid);

        Assert.Equal(OrderStatus.Paid, order.Status);
    }

    [Fact]
    public void UpdateStatus_AllowsAdvancingToShipped()
    {
        var order = Order.Create(Guid.NewGuid(), Money.Create(10m, "USD"));
        order.UpdateStatus(OrderStatus.Paid);

        order.UpdateStatus(OrderStatus.Shipped);

        Assert.Equal(OrderStatus.Shipped, order.Status);
    }

    [Fact]
    public void UpdateStatus_Throws_WhenSkippingToShipped()
    {
        var order = Order.Create(Guid.NewGuid(), Money.Create(10m, "USD"));

        Assert.Throws<InvalidOperationException>(() => order.UpdateStatus(OrderStatus.Shipped));
    }

    [Fact]
    public void UpdateStatus_Throws_WhenRevertingToNew()
    {
        var order = Order.Create(Guid.NewGuid(), Money.Create(10m, "USD"));
        order.UpdateStatus(OrderStatus.Paid);

        Assert.Throws<InvalidOperationException>(() => order.UpdateStatus(OrderStatus.New));
    }

    [Fact]
    public void UpdateStatus_AllowsIdempotentShipped()
    {
        var order = Order.Create(Guid.NewGuid(), Money.Create(10m, "USD"));
        order.UpdateStatus(OrderStatus.Paid);
        order.UpdateStatus(OrderStatus.Shipped);

        order.UpdateStatus(OrderStatus.Shipped);

        Assert.Equal(OrderStatus.Shipped, order.Status);
    }

    [Fact]
    public void UpdateStatus_IsIdempotent()
    {
        var order = Order.Create(Guid.NewGuid(), Money.Create(10m, "USD"));
        order.UpdateStatus(OrderStatus.Paid);

        order.UpdateStatus(OrderStatus.Paid);

        Assert.Equal(OrderStatus.Paid, order.Status);
    }
}
