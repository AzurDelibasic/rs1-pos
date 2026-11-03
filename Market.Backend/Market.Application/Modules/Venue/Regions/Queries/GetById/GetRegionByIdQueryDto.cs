namespace Market.Application.Modules.Venue.Regions.Queries.GetById;

public sealed class GetRegionByIdQueryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int SortOrder { get; init; }
    public required bool IsActive { get; init; }
}