namespace Market.Application.Modules.Venue.Tables.Commands.Update;

public sealed class UpdateTableCommand : IRequest<Unit>
{
    [JsonIgnore]
    public int Id { get; set; }
    public required string Label { get; set; }
    public required int Seats { get; set; }
    public required int RegionId { get; set; }
    public required bool IsActive { get; set; }
}
