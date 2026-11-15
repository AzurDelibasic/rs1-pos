using Market.Application.Common.Exceptions;
using Market.Application.Modules.Settings.Countries.Commands.Create;
using Market.Application.Modules.Settings.Countries.Commands.Update;

namespace Market.Tests.CountryTests.UnitTests;

public class CountryUnitTests
{
    private static DatabaseContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DatabaseContext(options, new Microsoft.Extensions.Time.Testing.FakeTimeProvider());
    }

    [Fact]
    public async Task Create_ShouldSaveCountry_WithTrimmedNameAndUppercaseCode()
    {
        using var context = GetInMemoryDbContext();
        var handler = new CreateCountryCommandHandler(context);

        var id = await handler.Handle(
            new CreateCountryCommand { Name = "  Bosna i Hercegovina  ", Code = "ba", VatRate = 17 },
            CancellationToken.None);

        var country = await context.Countries.FindAsync(id);
        Assert.NotNull(country);
        Assert.Equal("Bosna i Hercegovina", country!.Name);
        Assert.Equal("BA", country.Code);
        Assert.Equal(17m, country.VatRate);
    }

    [Fact]
    public async Task Create_ShouldThrowConflict_WhenCodeAlreadyExists()
    {
        using var context = GetInMemoryDbContext();
        var handler = new CreateCountryCommandHandler(context);
        await handler.Handle(new CreateCountryCommand { Name = "Hrvatska", Code = "HR", VatRate = 25 }, CancellationToken.None);

        await Assert.ThrowsAsync<MarketConflictException>(() =>
            handler.Handle(new CreateCountryCommand { Name = "Croatia", Code = "hr", VatRate = 25 }, CancellationToken.None));
    }

    [Fact]
    public async Task Update_ShouldThrowConflict_WhenNameBelongsToOtherCountry()
    {
        using var context = GetInMemoryDbContext();
        var createHandler = new CreateCountryCommandHandler(context);
        await createHandler.Handle(new CreateCountryCommand { Name = "Srbija", Code = "RS", VatRate = 20 }, CancellationToken.None);
        var secondId = await createHandler.Handle(new CreateCountryCommand { Name = "Crna Gora", Code = "ME", VatRate = 21 }, CancellationToken.None);

        var handler = new UpdateCountryCommandHandler(context);

        await Assert.ThrowsAsync<MarketConflictException>(() =>
            handler.Handle(new UpdateCountryCommand { Id = secondId, Name = "srbija", Code = "ME", VatRate = 21 }, CancellationToken.None));
    }
}
