using Market.Application.Abstractions;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Shifts.Shifts.Commands.Close;
using Market.Application.Modules.Shifts.Shifts.Commands.Open;
using Market.Application.Modules.Shifts.CashRegisters.Commands.Delete;
using Market.Application.Modules.Shifts.CashRegisters.Commands.Status.Deactivate;
using Market.Domain.Entities.Identity;
using Market.Domain.Entities.Shifts;
using Microsoft.Extensions.Time.Testing;

namespace Market.Tests.ShiftTests.UnitTests;

public class ShiftUnitTests
{
    private static DatabaseContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DatabaseContext(options, new FakeTimeProvider());
    }

    private static async Task<CashRegisterEntity> AddCashRegisterAsync(DatabaseContext context, string name, bool isActive = true)
    {
        var cashRegister = new CashRegisterEntity { Name = name, IsActive = isActive };
        context.CashRegisters.Add(cashRegister);
        await context.SaveChangesAsync();
        return cashRegister;
    }

    private static async Task<MarketUserEntity> AddUserAsync(DatabaseContext context, string email, bool isEmployee)
    {
        var user = new MarketUserEntity
        {
            Firstname = "Test",
            Lastname = "User",
            Email = email,
            PasswordHash = "hash",
            IsEmployee = isEmployee,
            IsEnabled = true
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    [Fact]
    public async Task Open_ByManager_ShouldOpenShiftForSelectedWaiter()
    {
        using var context = GetInMemoryDbContext();
        var cashRegister = await AddCashRegisterAsync(context, "Kasa 1");
        var waiter = await AddUserAsync(context, "konobar@test.ba", isEmployee: true);
        var handler = new OpenShiftCommandHandler(context, new FakeCurrentUser(99, isManager: true), new FakeTimeProvider());

        var id = await handler.Handle(
            new OpenShiftCommand { CashRegisterId = cashRegister.Id, OpeningCash = 100, UserId = waiter.Id },
            CancellationToken.None);

        var shift = await context.Shifts.FindAsync(id);
        Assert.Equal(waiter.Id, shift!.UserId);
    }

    [Fact]
    public async Task Open_ByManager_ShouldThrowBusinessRule_WhenSelectedUserIsNotWaiter()
    {
        using var context = GetInMemoryDbContext();
        var cashRegister = await AddCashRegisterAsync(context, "Kasa 1");
        var user = await AddUserAsync(context, "menadzer@test.ba", isEmployee: false);
        var handler = new OpenShiftCommandHandler(context, new FakeCurrentUser(99, isManager: true), new FakeTimeProvider());

        await Assert.ThrowsAsync<MarketBusinessRuleException>(() =>
            handler.Handle(
                new OpenShiftCommand { CashRegisterId = cashRegister.Id, OpeningCash = 100, UserId = user.Id },
                CancellationToken.None));

        Assert.False(await context.Shifts.AnyAsync());
    }

    [Fact]
    public async Task Open_ShouldThrowBusinessRule_WhenCashRegisterIsInactive()
    {
        using var context = GetInMemoryDbContext();
        var cashRegister = await AddCashRegisterAsync(context, "Kasa 3", isActive: false);
        var handler = new OpenShiftCommandHandler(context, new FakeCurrentUser(1), new FakeTimeProvider());

        await Assert.ThrowsAsync<MarketBusinessRuleException>(() =>
            handler.Handle(new OpenShiftCommand { CashRegisterId = cashRegister.Id, OpeningCash = 100 }, CancellationToken.None));

        Assert.False(await context.Shifts.AnyAsync());
    }

    [Fact]
    public async Task Open_ShouldThrowBusinessRule_WhenUserAlreadyHasOpenShift()
    {
        using var context = GetInMemoryDbContext();
        var first = await AddCashRegisterAsync(context, "Kasa 1");
        var second = await AddCashRegisterAsync(context, "Kasa 2");
        var handler = new OpenShiftCommandHandler(context, new FakeCurrentUser(1), new FakeTimeProvider());
        await handler.Handle(new OpenShiftCommand { CashRegisterId = first.Id, OpeningCash = 100 }, CancellationToken.None);

        await Assert.ThrowsAsync<MarketBusinessRuleException>(() =>
            handler.Handle(new OpenShiftCommand { CashRegisterId = second.Id, OpeningCash = 50 }, CancellationToken.None));

        Assert.Equal(1, await context.Shifts.CountAsync());
    }

    [Fact]
    public async Task Open_ShouldThrowBusinessRule_WhenCashRegisterHasOpenShift()
    {
        using var context = GetInMemoryDbContext();
        var cashRegister = await AddCashRegisterAsync(context, "Kasa 1");
        await new OpenShiftCommandHandler(context, new FakeCurrentUser(1), new FakeTimeProvider())
            .Handle(new OpenShiftCommand { CashRegisterId = cashRegister.Id, OpeningCash = 100 }, CancellationToken.None);

        var handler = new OpenShiftCommandHandler(context, new FakeCurrentUser(2), new FakeTimeProvider());

        await Assert.ThrowsAsync<MarketBusinessRuleException>(() =>
            handler.Handle(new OpenShiftCommand { CashRegisterId = cashRegister.Id, OpeningCash = 100 }, CancellationToken.None));
    }

    [Fact]
    public async Task Close_ShouldCalculateExpectedCashAndDifference()
    {
        using var context = GetInMemoryDbContext();
        var cashRegister = await AddCashRegisterAsync(context, "Kasa 1");
        var currentUser = new FakeCurrentUser(1);
        var id = await new OpenShiftCommandHandler(context, currentUser, new FakeTimeProvider())
            .Handle(new OpenShiftCommand { CashRegisterId = cashRegister.Id, OpeningCash = 200 }, CancellationToken.None);

        var handler = new CloseShiftCommandHandler(context, currentUser, new FakeTimeProvider());
        await handler.Handle(new CloseShiftCommand { Id = id, CountedCash = 195.50m }, CancellationToken.None);

        var shift = await context.Shifts.FindAsync(id);
        Assert.Equal(ShiftStatusType.Closed, shift!.Status);
        Assert.Equal(200m, shift.ExpectedCash);
        Assert.Equal(-4.50m, shift.Difference);
        Assert.NotNull(shift.ClosedAtUtc);
    }

    [Fact]
    public async Task Close_ShouldThrowForbidden_WhenWaiterClosesOtherUsersShift()
    {
        using var context = GetInMemoryDbContext();
        var cashRegister = await AddCashRegisterAsync(context, "Kasa 1");
        var id = await new OpenShiftCommandHandler(context, new FakeCurrentUser(1), new FakeTimeProvider())
            .Handle(new OpenShiftCommand { CashRegisterId = cashRegister.Id, OpeningCash = 100 }, CancellationToken.None);

        var handler = new CloseShiftCommandHandler(context, new FakeCurrentUser(2), new FakeTimeProvider());

        await Assert.ThrowsAsync<MarketForbiddenException>(() =>
            handler.Handle(new CloseShiftCommand { Id = id, CountedCash = 100 }, CancellationToken.None));
    }

    [Fact]
    public async Task DeactivateCashRegister_ShouldThrowBusinessRule_WhenShiftIsOpen()
    {
        using var context = GetInMemoryDbContext();
        var cashRegister = await AddCashRegisterAsync(context, "Kasa 1");
        await new OpenShiftCommandHandler(context, new FakeCurrentUser(1), new FakeTimeProvider())
            .Handle(new OpenShiftCommand { CashRegisterId = cashRegister.Id, OpeningCash = 100 }, CancellationToken.None);

        var handler = new DeactivateCashRegisterCommandHandler(context);

        await Assert.ThrowsAsync<MarketBusinessRuleException>(() =>
            handler.Handle(new DeactivateCashRegisterCommand { Id = cashRegister.Id }, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteCashRegister_ShouldThrowConflict_WhenCashRegisterHasShifts()
    {
        using var context = GetInMemoryDbContext();
        var cashRegister = await AddCashRegisterAsync(context, "Kasa 1");
        await new OpenShiftCommandHandler(context, new FakeCurrentUser(1), new FakeTimeProvider())
            .Handle(new OpenShiftCommand { CashRegisterId = cashRegister.Id, OpeningCash = 100 }, CancellationToken.None);

        var handler = new DeleteCashRegisterCommandHandler(context, new FakeCurrentUser(99, isAdmin: true));

        await Assert.ThrowsAsync<MarketConflictException>(() =>
            handler.Handle(new DeleteCashRegisterCommand { Id = cashRegister.Id }, CancellationToken.None));
    }

    private sealed class FakeCurrentUser(int userId, bool isAdmin = false, bool isManager = false) : IAppCurrentUser
    {
        public int? UserId => userId;
        public string? Email => null;
        public bool IsAuthenticated => true;
        public bool IsAdmin => isAdmin;
        public bool IsManager => isManager;
        public bool IsEmployee => !isAdmin && !isManager;
    }
}
