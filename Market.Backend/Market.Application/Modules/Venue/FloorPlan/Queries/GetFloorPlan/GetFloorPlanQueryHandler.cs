using Market.Domain.Entities.Venue;

namespace Market.Application.Modules.Venue.FloorPlan.Queries.GetFloorPlan;

public sealed class GetFloorPlanQueryHandler(IAppDbContext ctx)
    : IRequestHandler<GetFloorPlanQuery, GetFloorPlanQueryDto>
{
    public async Task<GetFloorPlanQueryDto> Handle(GetFloorPlanQuery request, CancellationToken ct)
    {
        var regions = await ctx.Regions
            .AsNoTracking()
            .Where(r => r.IsActive)
            .OrderBy(r => r.SortOrder)
            .ThenBy(r => r.Name)
            .ThenBy(r => r.Id)
            .Select(r => new GetFloorPlanQueryDtoRegion
            {
                Id = r.Id,
                Name = r.Name,
                Tables = r.Tables
                    .Where(t => t.IsActive)
                    .OrderBy(t => t.Label)
                    .ThenBy(t => t.Id)
                    .Select(t => new GetFloorPlanQueryDtoTable
                    {
                        Id = t.Id,
                        Label = t.Label,
                        Seats = t.Seats,
                        Status = TableStatusType.Free
                    })
                    .ToList()
            })
            .ToListAsync(ct);

        return new GetFloorPlanQueryDto
        {
            Regions = regions
        };
    }
}
