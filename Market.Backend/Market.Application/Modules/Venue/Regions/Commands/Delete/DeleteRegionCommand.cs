namespace Market.Application.Modules.Venue.Regions.Commands.Delete;

public sealed class DeleteRegionCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}