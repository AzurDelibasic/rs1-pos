using Market.Application.Common.Exceptions;
using Market.Application.Modules.Venue.Regions.Commands.Create;

namespace Market.Tests.RegionTests.UnitTests;

public class RegionUnitTests
{
    private static DatabaseContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DatabaseContext(options, new Microsoft.Extensions.Time.Testing.FakeTimeProvider());
    }

    [Fact]
    public async Task Create_ShouldSaveRegion_WithTrimmedName()
    {
        using var context = GetInMemoryDbContext();
        var handler = new CreateRegionCommandHandler(context);
        var command = new CreateRegionCommand { Name = "  Prvi dio  ", SortOrder = 2 };

        var id = await handler.Handle(command, CancellationToken.None);

        var region = await context.Regions.FindAsync(id);
        Assert.NotNull(region);
        Assert.Equal("Prvi dio", region!.Name);
        Assert.Equal(2, region.SortOrder);
        Assert.True(region.IsActive);
    }

    [Fact]
    public async Task Create_ShouldThrowConflict_WhenNameAlreadyExists()
    {
        using var context = GetInMemoryDbContext();
        var handler = new CreateRegionCommandHandler(context);
        await handler.Handle(new CreateRegionCommand { Name = "Sala" }, CancellationToken.None);

        await Assert.ThrowsAsync<MarketConflictException>(() =>
            handler.Handle(new CreateRegionCommand { Name = "sala" }, CancellationToken.None));
    }
}
