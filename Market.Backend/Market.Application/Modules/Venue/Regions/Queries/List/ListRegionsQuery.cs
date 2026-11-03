namespace Market.Application.Modules.Venue.Regions.Queries.List
{
    public sealed class ListRegionsQuery : BasePagedQuery<ListRegionsQueryDto>
    {
        public string? Search {  get; set; }
        public bool? OnlyActive { get; set; }
    }
}
