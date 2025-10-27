using MediatR;
using SolidApiExample.Application.Repositories;

namespace SolidApiExample.Application.People.DeletePerson;

public sealed record DeletePersonCommand(Guid Id) : IRequest<Unit>;

public sealed class DeletePersonHandler : IRequestHandler<DeletePersonCommand, Unit>
{
    private readonly IPeopleRepo _repo;

    public DeletePersonHandler(IPeopleRepo repo) => _repo = repo;

    public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
