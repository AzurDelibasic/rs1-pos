namespace Market.Application.Modules.Settings.Countries.Commands.Delete;

public sealed class DeleteCountryCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
