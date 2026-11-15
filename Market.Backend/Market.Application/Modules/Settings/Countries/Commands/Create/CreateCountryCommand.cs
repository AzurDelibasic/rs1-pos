namespace Market.Application.Modules.Settings.Countries.Commands.Create;

public sealed class CreateCountryCommand : IRequest<int>
{
    public required string Name { get; set; }

    public required string Code { get; set; }

    public required decimal VatRate { get; set; }
}
