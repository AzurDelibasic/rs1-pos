namespace Market.Application.Modules.Shifts.CashRegisters.Queries.List;

public sealed class ListCashRegistersQueryHandler(IAppDbContext ctx)
    : IRequestHandler<ListCashRegistersQuery, PageResult<ListCashRegistersQueryDto>>
{
    public async Task<PageResult<ListCashRegistersQueryDto>> Handle(ListCashRegistersQuery request, CancellationToken ct)
    {
        var q = ctx.CashRegisters.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            q = q.Where(x => x.Name.Contains(request.Search));

        if (request.OnlyActive is not null)
            q = q.Where(x => x.IsActive == request.OnlyActive);

        var projected = q
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Id)
            .Select(x => new ListCashRegistersQueryDto
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive
            });

        return await PageResult<ListCashRegistersQueryDto>.FromQueryableAsync(projected, request.Paging, ct);
    }
}
