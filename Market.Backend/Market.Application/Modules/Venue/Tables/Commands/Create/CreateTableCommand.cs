namespace Market.Application.Modules.Venue.Tables.Commands.Create;

public sealed class CreateTableCommand : IRequest<int>
{
    public required string Label { get; set; }
    public required int Seats { get; set; }
    public required int RegionId { get; set; }
    public bool IsActive { get; set; } = true;
}
