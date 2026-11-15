namespace Market.Application.Modules.Settings.Countries.Queries.List;

public sealed class ListCountriesQuery : BasePagedQuery<ListCountriesQueryDto>
{
    public string? Search { get; set; }
}
