namespace Market.Application.Modules.Venue.Regions.Queries.GetById;

public sealed class GetRegionByIdQueryHandler(IAppDbContext ctx)
    : IRequestHandler<GetRegionByIdQuery, GetRegionByIdQueryDto>
{
    public async Task<GetRegionByIdQueryDto> Handle(GetRegionByIdQuery request, CancellationToken ct)
    {
        var region = await ctx.Regions
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new GetRegionByIdQueryDto
            {
                Id = x.Id,
                Name = x.Name,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(ct);

        return region ?? throw new MarketNotFoundException("Regija nije pronađena.");
    }
}