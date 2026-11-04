namespace Market.Application.Modules.Venue.Tables.Queries.GetById;

public sealed class GetTableByIdQueryHandler(IAppDbContext context)
    : IRequestHandler<GetTableByIdQuery, GetTableByIdQueryDto>
{
    public async Task<GetTableByIdQueryDto> Handle(GetTableByIdQuery request, CancellationToken cancellationToken)
    {
        var table = await context.Tables
            .Where(x => x.Id == request.Id)
            .Select(x => new GetTableByIdQueryDto
            {
                Id = x.Id,
                Label = x.Label,
                Seats = x.Seats,
                RegionId = x.RegionId,
                RegionName = x.Region!.Name,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (table == null)
        {
            throw new MarketNotFoundException($"Sto (ID={request.Id}) nije pronađen.");
        }

        return table;
    }
}
