namespace Market.Application.Modules.Venue.Regions.Commands.Status.Deactivate;

public sealed class DeactivateRegionCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
