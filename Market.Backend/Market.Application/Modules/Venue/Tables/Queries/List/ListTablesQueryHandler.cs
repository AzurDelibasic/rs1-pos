namespace Market.Application.Modules.Venue.Tables.Queries.List;

public sealed class ListTablesQueryHandler(IAppDbContext ctx)
    : IRequestHandler<ListTablesQuery, PageResult<ListTablesQueryDto>>
{
    public async Task<PageResult<ListTablesQueryDto>> Handle(ListTablesQuery request, CancellationToken ct)
    {
        var q = ctx.Tables.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var searchTerm = request.Search.Trim().ToLower();
            q = q.Where(x => x.Label.ToLower().Contains(searchTerm));
        }

        if (request.RegionId is not null)
            q = q.Where(x => x.RegionId == request.RegionId);

        if (request.OnlyActive is not null)
            q = q.Where(x => x.IsActive == request.OnlyActive);

        var projected = q
            .OrderBy(x => x.Region!.SortOrder)
            .ThenBy(x => x.Region!.Name)
            .ThenBy(x => x.Label)
            .ThenBy(x => x.Id)
            .Select(x => new ListTablesQueryDto
            {
                Id = x.Id,
                Label = x.Label,
                Seats = x.Seats,
                RegionId = x.RegionId,
                RegionName = x.Region!.Name,
                IsActive = x.IsActive
            });

        return await PageResult<ListTablesQueryDto>.FromQueryableAsync(projected, request.Paging, ct);
    }
}
