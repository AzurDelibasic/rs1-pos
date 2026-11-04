namespace Market.Application.Modules.Venue.Tables.Queries.List;

public sealed class ListTablesQueryDto
{
    public required int Id { get; init; }
    public required string Label { get; init; }
    public required int Seats { get; init; }
    public required int RegionId { get; init; }
    public required string RegionName { get; init; }
    public required bool IsActive { get; init; }
}
