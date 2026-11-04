using Market.Application.Common.Exceptions;
using Market.Application.Modules.Venue.Regions.Commands.Status.Deactivate;
using Market.Application.Modules.Venue.Tables.Commands.Create;
using Market.Domain.Entities.Venue;

namespace Market.Tests.TableTests.UnitTests;

public class TableUnitTests
{
    private static DatabaseContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DatabaseContext(options, new Microsoft.Extensions.Time.Testing.FakeTimeProvider());
    }

    [Fact]
    public async Task Create_ShouldThrowBusinessRule_WhenRegionIsInactive()
    {
        using var context = GetInMemoryDbContext();
        var region = new RegionEntity { Name = "Bašta", SortOrder = 1, IsActive = false };
        context.Regions.Add(region);
        await context.SaveChangesAsync();

        var handler = new CreateTableCommandHandler(context);
        var command = new CreateTableCommand { Label = "B1", Seats = 4, RegionId = region.Id };

        await Assert.ThrowsAsync<MarketBusinessRuleException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.False(await context.Tables.AnyAsync());
    }

    [Fact]
    public async Task DeactivateRegion_ShouldThrowBusinessRule_WhenRegionHasActiveTables()
    {
        using var context = GetInMemoryDbContext();
        var region = new RegionEntity { Name = "Sala", SortOrder = 1, IsActive = true };
        context.Regions.Add(region);
        await context.SaveChangesAsync();

        var tableHandler = new CreateTableCommandHandler(context);
        await tableHandler.Handle(new CreateTableCommand { Label = "S1", Seats = 4, RegionId = region.Id }, CancellationToken.None);

        var handler = new DeactivateRegionCommandHandler(context);

        await Assert.ThrowsAsync<MarketBusinessRuleException>(() =>
            handler.Handle(new DeactivateRegionCommand { Id = region.Id }, CancellationToken.None));

        Assert.True((await context.Regions.FindAsync(region.Id))!.IsActive);
    }
}
