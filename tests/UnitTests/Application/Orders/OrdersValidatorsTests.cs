using SolidApiExample.Application.Orders.CreateOrder;
using SolidApiExample.Application.Orders.GetOrder;
using SolidApiExample.Application.Orders.ListOrders;
using SolidApiExample.Application.Orders.UpdateOrder;

namespace SolidApiExample.UnitTests.Application.Orders;

public sealed class OrdersValidatorsTests
{
    [Fact]
    public void CreateOrderValidator_ReturnsFailure_WhenPersonIdIsEmpty()
    {
        var validator = new CreateOrderValidator();
        var dto = new CreateOrderDto { PersonId = Guid.Empty, Status = "New" };

        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains("PersonId must be a non-empty GUID.", result.Errors);
    }

    [Fact]
    public void CreateOrderValidator_ReturnsFailure_WhenStatusMissing()
    {
        var validator = new CreateOrderValidator();
        var dto = new CreateOrderDto { PersonId = Guid.NewGuid(), Status = " " };

        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains("Status must be provided.", result.Errors);
    }

    [Fact]
    public void CreateOrderValidator_ReturnsSuccess_ForValidRequest()
    {
        var validator = new CreateOrderValidator();
        var dto = new CreateOrderDto { PersonId = Guid.NewGuid(), Status = "Pending" };

        var result = validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void UpdateOrderValidator_ReturnsFailure_WhenStatusMissing()
    {
        var validator = new UpdateOrderValidator();
        var dto = new UpdateOrderDto { Status = "" };

        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains("Status must be provided.", result.Errors);
    }

    [Fact]
    public void UpdateOrderValidator_ReturnsSuccess_WhenStatusProvided()
    {
        var validator = new UpdateOrderValidator();
        var dto = new UpdateOrderDto { Status = "Completed" };

        var result = validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void GetOrderValidator_ReturnsFailure_WhenIdEmpty()
    {
        var validator = new GetOrderValidator();

        var result = validator.Validate(Guid.Empty);

        Assert.False(result.IsValid);
        Assert.Contains("Id must be a non-empty GUID.", result.Errors);
    }

    [Fact]
    public void GetOrderValidator_ReturnsSuccess_WhenIdProvided()
    {
        var validator = new GetOrderValidator();

        var result = validator.Validate(Guid.NewGuid());

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ListOrdersValidator_ReturnsFailure_WhenPageNegative()
    {
        var validator = new ListOrdersValidator();

        var result = validator.Validate((-1, 10));

        Assert.False(result.IsValid);
        Assert.Contains("Page must be zero or greater.", result.Errors);
    }

    [Fact]
    public void ListOrdersValidator_ReturnsFailure_WhenSizeNotPositive()
    {
        var validator = new ListOrdersValidator();

        var result = validator.Validate((0, 0));

        Assert.False(result.IsValid);
        Assert.Contains("Size must be greater than zero.", result.Errors);
    }

    [Fact]
    public void ListOrdersValidator_ReturnsSuccess_ForValidArguments()
    {
        var validator = new ListOrdersValidator();

        var result = validator.Validate((1, 10));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
