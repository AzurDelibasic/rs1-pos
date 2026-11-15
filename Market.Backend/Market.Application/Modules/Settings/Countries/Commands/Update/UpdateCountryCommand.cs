namespace Market.Application.Modules.Settings.Countries.Commands.Update;

public sealed class UpdateCountryCommand : IRequest<Unit>
{
    [JsonIgnore]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required decimal VatRate { get; set; }
}
