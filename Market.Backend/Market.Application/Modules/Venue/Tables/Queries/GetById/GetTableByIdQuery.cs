namespace Market.Application.Modules.Venue.Tables.Queries.GetById;

public sealed class GetTableByIdQuery : IRequest<GetTableByIdQueryDto>
{
    public required int Id { get; set; }
}
