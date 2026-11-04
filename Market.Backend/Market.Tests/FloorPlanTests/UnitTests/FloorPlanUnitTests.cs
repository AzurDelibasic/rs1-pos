using Market.Application.Modules.Venue.FloorPlan.Queries.GetFloorPlan;
using Market.Domain.Entities.Venue;

namespace Market.Tests.FloorPlanTests.UnitTests;

public class FloorPlanUnitTests
{
    private static DatabaseContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DatabaseContext(options, new Microsoft.Extensions.Time.Testing.FakeTimeProvider());
    }

    [Fact]
    public async Task Get_ShouldReturnOnlyActiveRegionsAndTables_InSortOrder()
    {
        using var context = GetInMemoryDbContext();
        var terrace = new RegionEntity { Name = "Terasa", SortOrder = 2, IsActive = true };
        var hall = new RegionEntity { Name = "Sala", SortOrder = 1, IsActive = true };
        var garden = new RegionEntity { Name = "Bašta", SortOrder = 3, IsActive = false };
        context.Regions.AddRange(terrace, hall, garden);
        await context.SaveChangesAsync();

        context.Tables.AddRange(
            new TableEntity { Label = "S2", Seats = 4, RegionId = hall.Id, IsActive = true },
            new TableEntity { Label = "S1", Seats = 4, RegionId = hall.Id, IsActive = true },
            new TableEntity { Label = "S3", Seats = 4, RegionId = hall.Id, IsActive = false },
            new TableEntity { Label = "T1", Seats = 6, RegionId = terrace.Id, IsActive = true },
            new TableEntity { Label = "B1", Seats = 2, RegionId = garden.Id, IsActive = true });
        await context.SaveChangesAsync();

        var handler = new GetFloorPlanQueryHandler(context);

        var result = await handler.Handle(new GetFloorPlanQuery(), CancellationToken.None);

        Assert.Equal(new[] { "Sala", "Terasa" }, result.Regions.Select(r => r.Name));
        Assert.Equal(new[] { "S1", "S2" }, result.Regions[0].Tables.Select(t => t.Label));
        Assert.All(result.Regions.SelectMany(r => r.Tables), t => Assert.Equal(TableStatusType.Free, t.Status));
    }
}
