namespace Market.Application.Modules.Venue.Tables.Queries.List;

public sealed class ListTablesQuery : BasePagedQuery<ListTablesQueryDto>
{
    public string? Search { get; init; }
    public int? RegionId { get; init; }
    public bool? OnlyActive { get; init; }
}
