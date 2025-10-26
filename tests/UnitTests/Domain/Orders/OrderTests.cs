using System;
using SolidApiExample.Domain.Orders;
using Xunit;

namespace SolidApiExample.UnitTests.Domain.Orders;

public sealed class OrderTests
{
    [Fact]
    public void Create_WithEmptyPersonId_Throws()
    {
        Assert.Throws<ArgumentException>(() => Order.Create(Guid.Empty, OrderStatus.Pending));
    }

    [Fact]
    public void FromExisting_WithEmptyId_Throws()
    {
        Assert.Throws<ArgumentException>(() => Order.FromExisting(Guid.Empty, Guid.NewGuid(), OrderStatus.Pending));
    }

    [Fact]
    public void UpdateStatus_ChangesValue()
    {
        var order = Order.Create(Guid.NewGuid(), OrderStatus.Pending);

        order.UpdateStatus(OrderStatus.Completed);

        Assert.Equal(OrderStatus.Completed, order.Status);
    }
}
