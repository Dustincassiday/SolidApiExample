using MediatR;
using SolidApiExample.Application.People;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.People.CreatePerson;

public sealed record CreatePersonCommand(CreatePersonDto Dto) : IRequest<PersonDto>;

public sealed class CreatePersonHandler : IRequestHandler<CreatePersonCommand, PersonDto>
{
    private readonly IPeopleRepo _repo;

    public CreatePersonHandler(IPeopleRepo repo) => _repo = repo;

    public async Task<PersonDto> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = request.Dto.ToPerson();
        var created = await _repo.AddAsync(person, cancellationToken);
        return created.ToDto();
    }
}
