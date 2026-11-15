namespace Market.Application.Modules.Settings.Countries.Queries.List;

public sealed class ListCountriesQueryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Code { get; init; }
    public required decimal VatRate { get; init; }
}
