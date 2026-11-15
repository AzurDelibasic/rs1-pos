using Market.Application.Abstractions;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Settings.AppSettings.Commands.Update;
using Market.Application.Modules.Settings.Countries.Commands.Delete;
using Market.Domain.Entities.Finance;
using Market.Domain.Entities.Settings;

namespace Market.Tests.AppSettingsTests.UnitTests;

public class AppSettingsUnitTests
{
    private static DatabaseContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DatabaseContext(options, new Microsoft.Extensions.Time.Testing.FakeTimeProvider());
    }

    private static async Task<(CountryEntity Country, CurrencyEntity Currency)> AddCountryAndCurrencyAsync(DatabaseContext context)
    {
        var country = new CountryEntity { Name = "Bosna i Hercegovina", Code = "BA", VatRate = 17m };
        var currency = new CurrencyEntity { Code = "BAM", Name = "Konvertibilna marka", Symbol = "KM", DecimalPlaces = 2, IsBase = true, IsEnabled = true };
        context.Countries.Add(country);
        context.Currencies.Add(currency);
        await context.SaveChangesAsync();
        return (country, currency);
    }

    private static UpdateAppSettingsCommand CreateCommand(int countryId) => new()
    {
        BusinessName = "  Kafić RS1  ",
        Address = "  ",
        TaxNumber = "4200000000000",
        CountryId = countryId,
        DefaultCurrencyCode = "bam",
        RoundingMode = RoundingModeType.NearestFiveCents,
        ReservationHoldMinutes = 15
    };

    [Fact]
    public async Task Update_ShouldCreateSettings_WhenNoneExist()
    {
        using var context = GetInMemoryDbContext();
        var (country, currency) = await AddCountryAndCurrencyAsync(context);
        var handler = new UpdateAppSettingsCommandHandler(context);

        await handler.Handle(CreateCommand(country.Id), CancellationToken.None);

        var settings = await context.AppSettings.SingleAsync();
        Assert.Equal("Kafić RS1", settings.BusinessName);
        Assert.Null(settings.Address);
        Assert.Equal(country.Id, settings.CountryId);
        Assert.Equal(currency.Id, settings.DefaultCurrencyId);
    }

    [Fact]
    public async Task Update_ShouldThrowNotFound_WhenCountryDoesNotExist()
    {
        using var context = GetInMemoryDbContext();
        await AddCountryAndCurrencyAsync(context);
        var handler = new UpdateAppSettingsCommandHandler(context);

        await Assert.ThrowsAsync<MarketNotFoundException>(() =>
            handler.Handle(CreateCommand(999), CancellationToken.None));

        Assert.False(await context.AppSettings.AnyAsync());
    }

    [Fact]
    public async Task DeleteCountry_ShouldThrowConflict_WhenCountryIsSelectedInSettings()
    {
        using var context = GetInMemoryDbContext();
        var (country, _) = await AddCountryAndCurrencyAsync(context);
        await new UpdateAppSettingsCommandHandler(context).Handle(CreateCommand(country.Id), CancellationToken.None);

        var handler = new DeleteCountryCommandHandler(context, new FakeAdmin());

        await Assert.ThrowsAsync<MarketConflictException>(() =>
            handler.Handle(new DeleteCountryCommand { Id = country.Id }, CancellationToken.None));
    }

    private sealed class FakeAdmin : IAppCurrentUser
    {
        public int? UserId => 1;
        public string? Email => null;
        public bool IsAuthenticated => true;
        public bool IsAdmin => true;
        public bool IsManager => false;
        public bool IsEmployee => false;
    }
}
