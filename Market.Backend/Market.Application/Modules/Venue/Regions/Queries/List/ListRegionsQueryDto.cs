namespace Market.Application.Modules.Venue.Regions.Queries.List
{
    public sealed class ListRegionsQueryDto
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required int SortOrder { get; init; }
        public required bool IsActive { get; init; }
    }
}
