using SolidApiExample.Application.Orders;
using SolidApiExample.Application.Orders.CreateOrder;
using SolidApiExample.Application.Orders.GetOrder;
using SolidApiExample.Application.Orders.ListOrders;
using SolidApiExample.Application.Orders.UpdateOrder;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.TestSuite.Application.Orders;

public sealed class OrdersValidatorsTests
{
    [Fact]
    public void CreateOrderValidator_ReturnsFailure_WhenCustomerIdIsEmpty()
    {
        var validator = new CreateOrderValidator();
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.Empty,
            Total = new MoneyDto { Amount = 10m, Currency = "USD" }
        };

        var result = validator.Validate(new CreateOrderCommand(dto));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, failure => failure.ErrorMessage == "CustomerId must be a non-empty GUID.");
    }

    [Fact]
    public void CreateOrderValidator_ReturnsFailure_WhenTotalMissing()
    {
        var validator = new CreateOrderValidator();
        var dto = new CreateOrderDto { CustomerId = Guid.NewGuid(), Total = null! };

        var result = validator.Validate(new CreateOrderCommand(dto));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, failure => failure.ErrorMessage == "Total must be provided.");
    }

    [Fact]
    public void CreateOrderValidator_ReturnsFailure_WhenAmountNegative()
    {
        var validator = new CreateOrderValidator();
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Total = new MoneyDto { Amount = -1m, Currency = "USD" }
        };

        var result = validator.Validate(new CreateOrderCommand(dto));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, failure => failure.ErrorMessage == "Total amount cannot be negative.");
    }

    [Fact]
    public void CreateOrderValidator_ReturnsFailure_WhenCurrencyInvalid()
    {
        var validator = new CreateOrderValidator();
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Total = new MoneyDto { Amount = 1m, Currency = "US" }
        };

        var result = validator.Validate(new CreateOrderCommand(dto));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, failure => failure.ErrorMessage == "Total currency must be a three-letter code.");
    }

    [Fact]
    public void CreateOrderValidator_ReturnsSuccess_ForValidRequest()
    {
        var validator = new CreateOrderValidator();
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Total = new MoneyDto { Amount = 10m, Currency = "USD" }
        };

        var result = validator.Validate(new CreateOrderCommand(dto));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void UpdateOrderValidator_ReturnsSuccess_WhenStatusProvided()
    {
        var validator = new UpdateOrderValidator();
        var dto = new UpdateOrderDto { Status = OrderStatusDto.Paid };

        var result = validator.Validate(new UpdateOrderCommand(Guid.NewGuid(), dto));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void UpdateOrderValidator_ReturnsFailure_WhenStatusInvalid()
    {
        var validator = new UpdateOrderValidator();
        var dto = new UpdateOrderDto { Status = (OrderStatusDto)999 };

        var result = validator.Validate(new UpdateOrderCommand(Guid.NewGuid(), dto));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, failure => failure.ErrorMessage == "Status must be provided.");
    }

    [Fact]
    public void GetOrderValidator_ReturnsFailure_WhenIdEmpty()
    {
        var validator = new GetOrderValidator();

        var result = validator.Validate(new GetOrderQuery(Guid.Empty));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, failure => failure.ErrorMessage == "Id must be a non-empty GUID.");
    }

    [Fact]
    public void GetOrderValidator_ReturnsSuccess_WhenIdProvided()
    {
        var validator = new GetOrderValidator();

        var result = validator.Validate(new GetOrderQuery(Guid.NewGuid()));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ListOrdersValidator_ReturnsFailure_WhenPageNegative()
    {
        var validator = new ListOrdersValidator();

        var result = validator.Validate(new ListOrdersQuery(-1, 10));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, failure => failure.ErrorMessage == "Page must be zero or greater.");
    }

    [Fact]
    public void ListOrdersValidator_ReturnsFailure_WhenSizeNotPositive()
    {
        var validator = new ListOrdersValidator();

        var result = validator.Validate(new ListOrdersQuery(0, 0));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, failure => failure.ErrorMessage == "Size must be greater than zero.");
    }

    [Fact]
    public void ListOrdersValidator_ReturnsSuccess_ForValidArguments()
    {
        var validator = new ListOrdersValidator();

        var result = validator.Validate(new ListOrdersQuery(1, 10));

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
