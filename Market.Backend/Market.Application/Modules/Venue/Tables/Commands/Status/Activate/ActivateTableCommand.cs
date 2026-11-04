namespace Market.Application.Modules.Venue.Tables.Commands.Status.Activate;

public sealed class ActivateTableCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
