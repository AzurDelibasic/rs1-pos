namespace Market.Application.Modules.Venue.Tables.Commands.Delete;

public sealed class DeleteTableCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
