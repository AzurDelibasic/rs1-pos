namespace Market.Application.Modules.Venue.Tables.Commands.Status.Deactivate;

public sealed class DeactivateTableCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
