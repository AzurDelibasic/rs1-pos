namespace Market.Application.Modules.Venue.Regions.Queries.GetById;

public sealed class GetRegionByIdQueryHandler(IAppDbContext context)
    : IRequestHandler<GetRegionByIdQuery, GetRegionByIdQueryDto>
{
    public async Task<GetRegionByIdQueryDto> Handle(GetRegionByIdQuery request, CancellationToken cancellationToken)
    {
        var region = await context.Regions
            .Where(x => x.Id == request.Id)
            .Select(x => new GetRegionByIdQueryDto
            {
                Id = x.Id,
                Name = x.Name,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (region == null)
        {
            throw new MarketNotFoundException($"Regija (ID={request.Id}) nije pronađena.");
        }

        return region;
    }
}
