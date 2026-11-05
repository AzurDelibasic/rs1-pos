using Market.Application.Common.Exceptions;
using Market.Application.Modules.Shifts.CashRegisters.Commands.Create;
using Market.Application.Modules.Shifts.CashRegisters.Commands.Update;

namespace Market.Tests.CashRegisterTests.UnitTests;

public class CashRegisterUnitTests
{
    private static DatabaseContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DatabaseContext(options, new Microsoft.Extensions.Time.Testing.FakeTimeProvider());
    }

    [Fact]
    public async Task Create_ShouldSaveCashRegister_WithTrimmedName()
    {
        using var context = GetInMemoryDbContext();
        var handler = new CreateCashRegisterCommandHandler(context);
        var command = new CreateCashRegisterCommand { Name = "  Kasa 1  " };

        var id = await handler.Handle(command, CancellationToken.None);

        var cashRegister = await context.CashRegisters.FindAsync(id);
        Assert.NotNull(cashRegister);
        Assert.Equal("Kasa 1", cashRegister!.Name);
        Assert.True(cashRegister.IsActive);
    }

    [Fact]
    public async Task Create_ShouldThrowConflict_WhenNameAlreadyExists()
    {
        using var context = GetInMemoryDbContext();
        var handler = new CreateCashRegisterCommandHandler(context);
        await handler.Handle(new CreateCashRegisterCommand { Name = "Kasa 1" }, CancellationToken.None);

        await Assert.ThrowsAsync<MarketConflictException>(() =>
            handler.Handle(new CreateCashRegisterCommand { Name = "kasa 1" }, CancellationToken.None));
    }

    [Fact]
    public async Task Update_ShouldThrowConflict_WhenNameBelongsToOtherCashRegister()
    {
        using var context = GetInMemoryDbContext();
        var createHandler = new CreateCashRegisterCommandHandler(context);
        await createHandler.Handle(new CreateCashRegisterCommand { Name = "Kasa 1" }, CancellationToken.None);
        var secondId = await createHandler.Handle(new CreateCashRegisterCommand { Name = "Kasa 2" }, CancellationToken.None);

        var handler = new UpdateCashRegisterCommandHandler(context);

        await Assert.ThrowsAsync<MarketConflictException>(() =>
            handler.Handle(new UpdateCashRegisterCommand { Id = secondId, Name = "Kasa 1", IsActive = true }, CancellationToken.None));
    }
}
