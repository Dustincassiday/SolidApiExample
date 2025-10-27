using MediatR;
using SolidApiExample.Application.People;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Application.People.ListPeople;

public sealed record ListPeopleQuery(int Page, int Size) : IRequest<Paged<PersonDto>>;

public sealed class ListPeopleHandler : IRequestHandler<ListPeopleQuery, Paged<PersonDto>>
{
    private readonly IPeopleRepo _repo;

    public ListPeopleHandler(IPeopleRepo repo) => _repo = repo;

    public async Task<Paged<PersonDto>> Handle(ListPeopleQuery request, CancellationToken cancellationToken)
    {
        var people = await _repo.ListAsync(request.Page, request.Size, cancellationToken);
        return people.ToDto();
    }
}
