namespace Market.Application.Modules.Settings.Countries.Queries.GetById;

public sealed class GetCountryByIdQueryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Code { get; init; }
    public required decimal VatRate { get; init; }
}
