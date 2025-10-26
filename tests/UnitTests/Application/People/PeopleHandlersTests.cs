using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using SolidApiExample.Application.People.CreatePerson;
using SolidApiExample.Application.People.DeletePerson;
using SolidApiExample.Application.People.GetPerson;
using SolidApiExample.Application.People.ListPeople;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.People.UpdatePerson;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;
using Xunit;

namespace SolidApiExample.UnitTests.Application.People;

public sealed class PeopleHandlersTests
{
    private readonly Mock<IPeopleRepo> _repoMock = new();

    [Fact]
    public async Task GetPerson_Returns_Person_WhenFound()
    {
        var personId = Guid.NewGuid();
        var expected = new PersonDto { Id = personId, Name = "Ada Lovelace" };

        _repoMock
            .Setup(m => m.FindAsync(personId, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new GetPerson(_repoMock.Object);

        var result = await handler.GetAsync(personId, CancellationToken.None);

        Assert.Same(expected, result);
        _repoMock.Verify(m => m.FindAsync(personId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetPerson_Throws_WhenNotFound()
    {
        var personId = Guid.NewGuid();

        _repoMock
            .Setup(m => m.FindAsync(personId, CancellationToken.None))
            .ReturnsAsync((PersonDto?)null);

        var handler = new GetPerson(_repoMock.Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.GetAsync(personId, CancellationToken.None));
        _repoMock.Verify(m => m.FindAsync(personId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ListPeople_Returns_PagedPeople()
    {
        const int page = 0;
        const int size = 20;
        var expected = new Paged<PersonDto>
        {
            Items = new List<PersonDto>
            {
                new() { Id = Guid.NewGuid(), Name = "Grace Hopper" }
            },
            Page = page,
            Size = size,
            Total = 1
        };

        _repoMock
            .Setup(m => m.ListAsync(page, size, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new ListPeople(_repoMock.Object);

        var result = await handler.ListAsync(page, size, CancellationToken.None);

        Assert.Same(expected, result);
        _repoMock.Verify(m => m.ListAsync(page, size, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CreatePerson_Forwards_ToRepository()
    {
        var dto = new CreatePersonDto { Name = "Alan Turing" };
        var expected = new PersonDto { Id = Guid.NewGuid(), Name = dto.Name };

        _repoMock
            .Setup(m => m.AddAsync(dto, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new CreatePerson(_repoMock.Object);

        var result = await handler.CreateAsync(dto, CancellationToken.None);

        Assert.Same(expected, result);
        _repoMock.Verify(m => m.AddAsync(dto, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task UpdatePerson_Forwards_ToRepository()
    {
        var personId = Guid.NewGuid();
        var dto = new UpdatePersonDto { Name = "Updated Name" };
        var expected = new PersonDto { Id = personId, Name = dto.Name };

        _repoMock
            .Setup(m => m.UpdateAsync(personId, dto, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new UpdatePerson(_repoMock.Object);

        var result = await handler.UpdateAsync(personId, dto, CancellationToken.None);

        Assert.Same(expected, result);
        _repoMock.Verify(m => m.UpdateAsync(personId, dto, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task DeletePerson_Forwards_ToRepository()
    {
        var personId = Guid.NewGuid();

        _repoMock
            .Setup(m => m.DeleteAsync(personId, CancellationToken.None))
            .Returns(Task.CompletedTask);

        var handler = new DeletePerson(_repoMock.Object);

        await handler.DeleteAsync(personId, CancellationToken.None);

        _repoMock.Verify(m => m.DeleteAsync(personId, CancellationToken.None), Times.Once);
    }
}
