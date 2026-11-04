using Market.Domain.Entities.Venue;

namespace Market.Application.Modules.Venue.FloorPlan.Queries.GetFloorPlan;

public sealed class GetFloorPlanQueryDto
{
    public required List<GetFloorPlanQueryDtoRegion> Regions { get; init; }
}

public sealed class GetFloorPlanQueryDtoRegion
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required List<GetFloorPlanQueryDtoTable> Tables { get; init; }
}

public sealed class GetFloorPlanQueryDtoTable
{
    public required int Id { get; init; }
    public required string Label { get; init; }
    public required int Seats { get; init; }
    public required TableStatusType Status { get; init; }
}
