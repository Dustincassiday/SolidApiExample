using System.Threading;
using System.Threading.Tasks;
using SolidApiExample.Application.Contracts;
using SolidApiExample.Application.People.Shared;
using SolidApiExample.Application.Repositories;
using SolidApiExample.Application.Shared;

namespace SolidApiExample.Application.People.ListPeople;

public sealed class ListPeople : IListItems<PersonDto>
{
    private readonly IPeopleRepo _repo;

    public ListPeople(IPeopleRepo repo) => _repo = repo;

    public Task<Paged<PersonDto>> ListAsync(int page, int size, CancellationToken ct = default) =>
        _repo.ListAsync(page, size, ct);
}
