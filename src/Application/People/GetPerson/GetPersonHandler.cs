using MediatR;
using SolidApiExample.Application.People;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.People.GetPerson;

public sealed record GetPersonQuery(Guid Id) : IRequest<PersonDto>;

public sealed class GetPersonHandler : IRequestHandler<GetPersonQuery, PersonDto>
{
    private readonly IPeopleRepo _repo;

    public GetPersonHandler(IPeopleRepo repo) => _repo = repo;

    public async Task<PersonDto> Handle(GetPersonQuery request, CancellationToken cancellationToken)
    {
        var person = await _repo.FindAsync(request.Id, cancellationToken) ??
                     throw new KeyNotFoundException("Person not found");
        return person.ToDto();
    }
}
