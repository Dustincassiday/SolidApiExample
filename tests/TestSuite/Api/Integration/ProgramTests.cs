using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using SolidApiExample.Api;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.Orders;
using SolidApiExample.Application.Customers.Shared;
using SolidApiExample.Api.Auth;

namespace SolidApiExample.TestSuite.Api.Integration;

public sealed class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProgramTests(WebApplicationFactory<Program> factory)
    {
        _client = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(logging => logging.ClearProviders());
                builder.UseSetting("https_port", "443");
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost")
            });
        _client.DefaultRequestHeaders.Add(ApiKeyDefaults.HeaderName, "dev-api-key");
    }

    [Fact]
    public async Task CreateOrder_ThenRetrieve_PersistsOrder()
    {
        // Arrange
        var createPayload = new
        {
            CustomerId = Guid.NewGuid(),
            Total = new { Amount = 19.99m, Currency = "USD" }
        };

        // Act
        var createResponse = await _client.PostAsJsonAsync("/api/orders", createPayload);

        // Assert
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<OrderDto>();
        Assert.NotNull(created);
        Assert.Equal(createPayload.CustomerId, created!.CustomerId);
        Assert.Equal(OrderStatusDto.New, created.Status);
        Assert.Equal(createPayload.Total.Amount, created.Total.Amount);
        Assert.Equal(createPayload.Total.Currency, created.Total.Currency);

        var getResponse = await _client.GetAsync($"/api/orders/{created.Id}");
        getResponse.EnsureSuccessStatusCode();
        var fetched = await getResponse.Content.ReadFromJsonAsync<OrderDto>();
        Assert.NotNull(fetched);
        Assert.Equal(created.Id, fetched!.Id);
        Assert.Equal(OrderStatusDto.New, fetched.Status);
    }

    [Fact]
    public async Task CreateCustomer_ThenRetrieve_PersistsCustomer()
    {
        // Arrange
        var createPayload = new { Name = "Ada Lovelace", Email = "ada@example.com" };

        // Act
        var createResponse = await _client.PostAsJsonAsync("/api/customers", createPayload);

        // Assert create
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<CustomerDto>();
        Assert.NotNull(created);
        Assert.Equal("Ada Lovelace", created!.Name);
        Assert.Equal("ada@example.com", created.Email);

        // Verify retrieval
        var getResponse = await _client.GetAsync($"/api/customers/{created.Id}");
        getResponse.EnsureSuccessStatusCode();
        var fetched = await getResponse.Content.ReadFromJsonAsync<CustomerDto>();
        Assert.NotNull(fetched);
        Assert.Equal(created.Id, fetched!.Id);
        Assert.Equal("Ada Lovelace", fetched.Name);
        Assert.Equal("ada@example.com", fetched.Email);
    }

    [Fact]
    public async Task MissingApiKey_ReturnsUnauthorized()
    {
        using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(logging => logging.ClearProviders());
                builder.UseSetting("https_port", "443");
            });

        var unauthenticatedClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });

        var response = await unauthenticatedClient.GetAsync("/api/customers");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
