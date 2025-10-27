
using Moq;
using SolidApiExample.Application.People.CreatePerson;
using SolidApiExample.Application.People.DeletePerson;
using SolidApiExample.Application.People.GetPerson;
using SolidApiExample.Application.People.ListPeople;
using SolidApiExample.Application.People.UpdatePerson;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;
using SolidApiExample.Domain.People;

namespace SolidApiExample.UnitTests.Application.People;

public sealed class PeopleHandlersTests
{
    private readonly Mock<IPeopleRepo> _repoMock = new();

    [Fact]
    public async Task GetPerson_Returns_Person_WhenFound()
    {
        var personId = Guid.NewGuid();
        var expected = Person.FromExisting(personId, "Ada Lovelace");

        _repoMock
            .Setup(m => m.FindAsync(personId, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new GetPerson(_repoMock.Object);

        var result = await handler.GetAsync(personId, CancellationToken.None);

        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(expected.Name, result.Name);
        _repoMock.Verify(m => m.FindAsync(personId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetPerson_Throws_WhenNotFound()
    {
        var personId = Guid.NewGuid();

        _repoMock
            .Setup(m => m.FindAsync(personId, CancellationToken.None))
            .ReturnsAsync((Person?)null);

        var handler = new GetPerson(_repoMock.Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.GetAsync(personId, CancellationToken.None));
        _repoMock.Verify(m => m.FindAsync(personId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ListPeople_Returns_PagedPeople()
    {
        const int page = 0;
        const int size = 20;
        var expected = new Paged<Person>
        {
            Items = new List<Person>
            {
                Person.FromExisting(Guid.NewGuid(), "Grace Hopper")
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

        Assert.Equal(expected.Total, result.Total);
        Assert.Equal(expected.Page, result.Page);
        Assert.Equal(expected.Size, result.Size);
        Assert.Single(result.Items);
        Assert.Equal(expected.Items.First().Name, result.Items.First().Name);
        _repoMock.Verify(m => m.ListAsync(page, size, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CreatePerson_Forwards_ToRepository()
    {
        var dto = new CreatePersonDto { Name = "Alan Turing" };
        _repoMock
            .Setup(m => m.AddAsync(It.IsAny<Person>(), CancellationToken.None))
            .ReturnsAsync((Person p, CancellationToken _) => Person.FromExisting(p.Id, p.Name));

        var handler = new CreatePerson(_repoMock.Object);

        var result = await handler.CreateAsync(dto, CancellationToken.None);

        Assert.Equal(dto.Name, result.Name);
        _repoMock.Verify(m => m.AddAsync(It.Is<Person>(p => p.Name == dto.Name), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task UpdatePerson_Forwards_ToRepository()
    {
        var personId = Guid.NewGuid();
        var dto = new UpdatePersonDto { Name = "Updated Name" };
        var expected = Person.FromExisting(personId, dto.Name);

        _repoMock
            .Setup(m => m.UpdateNameAsync(personId, dto.Name, CancellationToken.None))
            .ReturnsAsync(expected);

        var handler = new UpdatePerson(_repoMock.Object);

        var result = await handler.UpdateAsync(personId, dto, CancellationToken.None);

        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(expected.Name, result.Name);
        _repoMock.Verify(m => m.UpdateNameAsync(personId, dto.Name, CancellationToken.None), Times.Once);
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
