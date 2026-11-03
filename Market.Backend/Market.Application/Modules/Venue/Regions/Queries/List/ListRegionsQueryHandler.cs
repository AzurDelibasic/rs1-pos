namespace Market.Application.Modules.Venue.Regions.Queries.List
{
    public sealed class ListRegionsQueryHandler(IAppDbContext ctx)
        : IRequestHandler<ListRegionsQuery, PageResult<ListRegionsQueryDto>>
    {
        public async Task<PageResult<ListRegionsQueryDto>> Handle(ListRegionsQuery request, CancellationToken ct)
        {
            var q = ctx.Regions.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Search))
                q = q.Where(x => x.Name.Contains(request.Search));

            if (request.OnlyActive is not null)
                q = q.Where(x => x.IsActive == request.OnlyActive);

            var projected = q
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .ThenBy(x => x.Id)
                .Select(x => new ListRegionsQueryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    SortOrder = x.SortOrder,
                    IsActive = x.IsActive
                });

            return await PageResult<ListRegionsQueryDto>.FromQueryableAsync(projected, request.Paging, ct);
        }
    }
}
