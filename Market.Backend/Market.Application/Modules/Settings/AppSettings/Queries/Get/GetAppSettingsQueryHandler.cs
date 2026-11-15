namespace Market.Application.Modules.Settings.AppSettings.Queries.Get;

public sealed class GetAppSettingsQueryHandler(IAppDbContext context)
    : IRequestHandler<GetAppSettingsQuery, GetAppSettingsQueryDto>
{
    public async Task<GetAppSettingsQueryDto> Handle(GetAppSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = await context.AppSettings
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => new GetAppSettingsQueryDto
            {
                Id = x.Id,
                BusinessName = x.BusinessName,
                Address = x.Address,
                TaxNumber = x.TaxNumber,
                CountryId = x.CountryId,
                CountryName = x.Country!.Name,
                VatRate = x.Country.VatRate,
                DefaultCurrencyCode = x.DefaultCurrency!.Code,
                RoundingMode = x.RoundingMode,
                ReservationHoldMinutes = x.ReservationHoldMinutes
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (settings == null)
        {
            throw new MarketNotFoundException("Postavke aplikacije nisu podešene.");
        }

        return settings;
    }
}
