using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using SolidApiExample.Api;
using SolidApiExample.Application.Orders.Shared;
using SolidApiExample.Application.People.Shared;
using Xunit;

namespace SolidApiExample.UnitTests.Api.Integration;

public sealed class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProgramTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(_ => { }).CreateClient();
    }

    [Fact]
    public async Task CreateOrder_ThenRetrieve_PersistsOrder()
    {
        // Arrange
        var createPayload = new { PersonId = Guid.NewGuid(), Status = "Pending" };

        // Act
        var createResponse = await _client.PostAsJsonAsync("/api/orders", createPayload);

        // Assert
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<OrderDto>();
        Assert.NotNull(created);
        Assert.Equal(createPayload.PersonId, created!.PersonId);
        Assert.Equal("Pending", created.Status);

        var getResponse = await _client.GetAsync($"/api/orders/{created.Id}");
        getResponse.EnsureSuccessStatusCode();
        var fetched = await getResponse.Content.ReadFromJsonAsync<OrderDto>();
        Assert.NotNull(fetched);
        Assert.Equal(created.Id, fetched!.Id);
        Assert.Equal("Pending", fetched.Status);
    }

    [Fact]
    public async Task CreatePerson_ThenRetrieve_PersistsPerson()
    {
        // Arrange
        var createPayload = new { Name = "Ada Lovelace" };

        // Act
        var createResponse = await _client.PostAsJsonAsync("/api/people", createPayload);

        // Assert create
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<PersonDto>();
        Assert.NotNull(created);
        Assert.Equal("Ada Lovelace", created!.Name);

        // Verify retrieval
        var getResponse = await _client.GetAsync($"/api/people/{created.Id}");
        getResponse.EnsureSuccessStatusCode();
        var fetched = await getResponse.Content.ReadFromJsonAsync<PersonDto>();
        Assert.NotNull(fetched);
        Assert.Equal(created.Id, fetched!.Id);
        Assert.Equal("Ada Lovelace", fetched.Name);
    }
}
