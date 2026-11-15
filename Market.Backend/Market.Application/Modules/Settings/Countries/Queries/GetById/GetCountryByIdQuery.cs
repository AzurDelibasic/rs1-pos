namespace Market.Application.Modules.Settings.Countries.Queries.GetById;

public sealed class GetCountryByIdQuery : IRequest<GetCountryByIdQueryDto>
{
    public required int Id { get; set; }
}
