namespace Market.Application.Modules.Settings.Countries.Queries.List;

public sealed class ListCountriesQueryHandler(IAppDbContext ctx)
    : IRequestHandler<ListCountriesQuery, PageResult<ListCountriesQueryDto>>
{
    public async Task<PageResult<ListCountriesQueryDto>> Handle(ListCountriesQuery request, CancellationToken ct)
    {
        var q = ctx.Countries.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            q = q.Where(x => x.Name.Contains(request.Search) || x.Code.Contains(request.Search));

        var projected = q
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Id)
            .Select(x => new ListCountriesQueryDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                VatRate = x.VatRate
            });

        return await PageResult<ListCountriesQueryDto>.FromQueryableAsync(projected, request.Paging, ct);
    }
}
