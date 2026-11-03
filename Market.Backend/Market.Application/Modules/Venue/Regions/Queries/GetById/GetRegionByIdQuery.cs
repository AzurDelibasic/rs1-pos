namespace Market.Application.Modules.Venue.Regions.Queries.GetById;

public sealed class GetRegionByIdQuery : IRequest<GetRegionByIdQueryDto>
{
    public required int Id { get; set; }
}