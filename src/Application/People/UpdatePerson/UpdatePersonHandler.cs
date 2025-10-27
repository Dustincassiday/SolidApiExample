using MediatR;
using SolidApiExample.Application.People;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.People.UpdatePerson;

public sealed record UpdatePersonCommand(Guid Id, UpdatePersonDto Dto) : IRequest<PersonDto>;

public sealed class UpdatePersonHandler : IRequestHandler<UpdatePersonCommand, PersonDto>
{
    private readonly IPeopleRepo _repo;

    public UpdatePersonHandler(IPeopleRepo repo) => _repo = repo;

    public async Task<PersonDto> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var name = request.Dto.Name.ValidateAndNormalizeName();
        var updated = await _repo.UpdateNameAsync(request.Id, name, cancellationToken);
        return updated.ToDto();
    }
}
