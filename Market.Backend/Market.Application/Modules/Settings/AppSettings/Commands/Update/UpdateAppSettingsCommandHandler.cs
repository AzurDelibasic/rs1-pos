using Market.Domain.Entities.Settings;

namespace Market.Application.Modules.Settings.AppSettings.Commands.Update;

public sealed class UpdateAppSettingsCommandHandler(IAppDbContext context)
    : IRequestHandler<UpdateAppSettingsCommand, Unit>
{
    public async Task<Unit> Handle(UpdateAppSettingsCommand request, CancellationToken cancellationToken)
    {
        bool countryExists = await context.Countries
            .AnyAsync(x => x.Id == request.CountryId, cancellationToken);

        if (!countryExists)
            throw new MarketNotFoundException($"Država (ID={request.CountryId}) nije pronađena.");

        var currencyCode = request.DefaultCurrencyCode.Trim().ToUpperInvariant();

        var currency = await context.Currencies
            .FirstOrDefaultAsync(x => x.Code == currencyCode && x.IsEnabled, cancellationToken);

        if (currency is null)
            throw new MarketBusinessRuleException("settings.currency.notAvailable",
                $"Valuta {currencyCode} ne postoji ili nije omogućena.");

        var settings = await context.AppSettings
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (settings is null)
        {
            settings = new AppSettingsEntity();
            context.AppSettings.Add(settings);
        }

        settings.BusinessName = request.BusinessName.Trim();
        settings.Address = string.IsNullOrWhiteSpace(request.Address) ? null : request.Address.Trim();
        settings.TaxNumber = string.IsNullOrWhiteSpace(request.TaxNumber) ? null : request.TaxNumber.Trim();
        settings.CountryId = request.CountryId;
        settings.DefaultCurrencyId = currency.Id;
        settings.RoundingMode = request.RoundingMode;
        settings.ReservationHoldMinutes = request.ReservationHoldMinutes;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
