using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SolidApiExample.Api.Controllers;
using SolidApiExample.Application.People.CreatePerson;
using SolidApiExample.Application.People.DeletePerson;
using SolidApiExample.Application.People.GetPerson;
using SolidApiExample.Application.People.ListPeople;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.People.UpdatePerson;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.UnitTests.Controllers;

public sealed class PeopleControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();

    [Fact]
    public async Task Get_ReturnsPerson_FromHandler()
    {
        var personId = Guid.NewGuid();
        var expected = new PersonDto { Id = personId, Name = "Ada Lovelace" };
        var cancellation = CancellationToken.None;

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetPersonQuery>(q => q.Id == personId), cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Get(personId, cancellation);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _mediatorMock.Verify(m => m.Send(It.Is<GetPersonQuery>(q => q.Id == personId), cancellation), Times.Once);
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

        _mediatorMock
            .Setup(m => m.Send(It.Is<ListPeopleQuery>(q => q.Page == page && q.Size == size), cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.List(page, size, cancellation);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _mediatorMock.Verify(
            m => m.Send(It.Is<ListPeopleQuery>(q => q.Page == page && q.Size == size), cancellation),
            Times.Once);
    }

    [Fact]
    public async Task Create_ForwardsRequest_ToHandler()
    {
        var dto = new CreatePersonDto { Name = "Grace Hopper" };
        var expected = new PersonDto { Id = Guid.NewGuid(), Name = dto.Name };
        var cancellation = CancellationToken.None;

        _mediatorMock
            .Setup(m => m.Send(It.Is<CreatePersonCommand>(c => c.Dto == dto), cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Create(dto, cancellation);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(PeopleController.Get), created.ActionName);
        Assert.Equal(expected.Id, ((dynamic)created.RouteValues!)?["id"]);
        Assert.Same(expected, created.Value);
        _mediatorMock.Verify(m => m.Send(It.Is<CreatePersonCommand>(c => c.Dto == dto), cancellation), Times.Once);
    }

    [Fact]
    public async Task Update_ForwardsRequest_ToHandler()
    {
        var personId = Guid.NewGuid();
        var dto = new UpdatePersonDto { Name = "Updated Name" };
        var expected = new PersonDto { Id = personId, Name = dto.Name };
        var cancellation = CancellationToken.None;

        _mediatorMock
            .Setup(m => m.Send(It.Is<UpdatePersonCommand>(c => c.Id == personId && c.Dto == dto), cancellation))
            .ReturnsAsync(expected);

        var controller = CreateController();

        var result = await controller.Update(personId, dto, cancellation);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        _mediatorMock.Verify(
            m => m.Send(It.Is<UpdatePersonCommand>(c => c.Id == personId && c.Dto == dto), cancellation),
            Times.Once);
    }

    [Fact]
    public async Task Delete_ForwardsRequest_ToHandler()
    {
        var personId = Guid.NewGuid();
        var cancellation = CancellationToken.None;

        _mediatorMock
            .Setup(m => m.Send(It.Is<DeletePersonCommand>(c => c.Id == personId), cancellation))
            .ReturnsAsync(Unit.Value);

        var controller = CreateController();

        var result = await controller.Delete(personId, cancellation);

        Assert.IsType<NoContentResult>(result);
        _mediatorMock.Verify(m => m.Send(It.Is<DeletePersonCommand>(c => c.Id == personId), cancellation), Times.Once);
    }

    private PeopleController CreateController() => new(_mediatorMock.Object);
}
