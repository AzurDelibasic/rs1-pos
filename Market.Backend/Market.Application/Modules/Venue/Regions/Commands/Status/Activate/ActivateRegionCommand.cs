namespace Market.Application.Modules.Venue.Regions.Commands.Status.Activate;

public sealed class ActivateRegionCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
