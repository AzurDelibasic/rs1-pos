namespace Market.Application.Modules.Settings.Countries.Queries.GetById;

public sealed class GetCountryByIdQueryHandler(IAppDbContext context)
    : IRequestHandler<GetCountryByIdQuery, GetCountryByIdQueryDto>
{
    public async Task<GetCountryByIdQueryDto> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
    {
        var country = await context.Countries
            .Where(x => x.Id == request.Id)
            .Select(x => new GetCountryByIdQueryDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                VatRate = x.VatRate
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (country == null)
        {
            throw new MarketNotFoundException($"Država (ID={request.Id}) nije pronađena.");
        }

        return country;
    }
}
