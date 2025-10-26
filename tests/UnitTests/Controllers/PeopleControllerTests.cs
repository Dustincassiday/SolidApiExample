using Microsoft.AspNetCore.Mvc;
using Moq;
using SolidApiExample.Api.Controllers;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.People.CreatePerson;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.People.UpdatePerson;
using SolidApiExample.Application.Shared;


namespace SolidApiExample.UnitTests.Controllers;

public sealed class PeopleControllerTests
{
    private readonly Mock<IGetById<Guid, PersonDto>> _getMock = new();
    private readonly Mock<IListItems<PersonDto>> _listMock = new();
    private readonly Mock<ICreate<CreatePersonDto, PersonDto>> _createMock = new();
    private readonly Mock<IUpdate<Guid, UpdatePersonDto, PersonDto>> _updateMock = new();
    private readonly Mock<IDelete<Guid>> _deleteMock = new();

    [Fact]
    public async Task Get_ReturnsPerson_FromHandler()
    {
        var personId = Guid.NewGuid();
        var expected = new PersonDto { Id = personId, Name = "Ada Lovelace" };
        var cancellation = CancellationToken.None;

        _getMock
            .Setup(m => m.GetAsync(personId, cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Get(personId, cancellation);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _getMock.Verify(m => m.GetAsync(personId, cancellation), Times.Once);
    }

    [Fact]
    public async Task List_ReturnsPagedResult_FromHandler()
    {
        const int page = 1;
        const int size = 10;
        var cancellation = CancellationToken.None;
        var people = new List<PersonDto>
        {
            new() { Id = Guid.NewGuid(), Name = "Ada Lovelace" },
            new() { Id = Guid.NewGuid(), Name = "Alan Turing" }
        };
        var expected = new Paged<PersonDto>
        {
            Items = people,
            Page = page,
            Size = size,
            Total = 50
        };

        _listMock
            .Setup(m => m.ListAsync(page, size, cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.List(page, size, cancellation);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _listMock.Verify(m => m.ListAsync(page, size, cancellation), Times.Once);
    }

    [Fact]
    public async Task Create_ForwardsRequest_ToHandler()
    {
        var dto = new CreatePersonDto { Name = "Grace Hopper" };
        var expected = new PersonDto { Id = Guid.NewGuid(), Name = dto.Name };
        var cancellation = CancellationToken.None;

        _createMock
            .Setup(m => m.CreateAsync(dto, cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Create(dto, cancellation);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(PeopleController.Get), created.ActionName);
        Assert.Equal(expected.Id, ((dynamic)created.RouteValues!)?["id"]);
        Assert.Same(expected, created.Value);
        _createMock.Verify(m => m.CreateAsync(dto, cancellation), Times.Once);
    }

    [Fact]
    public async Task Update_ForwardsRequest_ToHandler()
    {
        var personId = Guid.NewGuid();
        var dto = new UpdatePersonDto { Name = "Updated Name" };
        var expected = new PersonDto { Id = personId, Name = dto.Name };
        var cancellation = CancellationToken.None;

        _updateMock
            .Setup(m => m.UpdateAsync(personId, dto, cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Update(personId, dto, cancellation);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _updateMock.Verify(m => m.UpdateAsync(personId, dto, cancellation), Times.Once);
    }

    [Fact]
    public async Task Delete_ForwardsRequest_ToHandler()
    {
        var personId = Guid.NewGuid();
        var cancellation = CancellationToken.None;

        _deleteMock
            .Setup(m => m.DeleteAsync(personId, cancellation))
            .Returns(Task.CompletedTask);

        var controller = CreateController();

        var result = await controller.Delete(personId, cancellation);

        Assert.IsType<NoContentResult>(result);
        _deleteMock.Verify(m => m.DeleteAsync(personId, cancellation), Times.Once);
    }

    private PeopleController CreateController() =>
        new(_getMock.Object, _listMock.Object, _createMock.Object, _updateMock.Object, _deleteMock.Object);
}
