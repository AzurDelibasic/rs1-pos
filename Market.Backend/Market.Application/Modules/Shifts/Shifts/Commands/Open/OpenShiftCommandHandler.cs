using Market.Domain.Entities.Shifts;

namespace Market.Application.Modules.Shifts.Shifts.Commands.Open;

public sealed class OpenShiftCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser, TimeProvider timeProvider)
    : IRequestHandler<OpenShiftCommand, int>
{
    public async Task<int> Handle(OpenShiftCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = appCurrentUser.UserId
            ?? throw new MarketUnauthorizedException("Korisnik nije prijavljen.");

        int userId;

        if (appCurrentUser.IsAdmin || appCurrentUser.IsManager)
        {
            if (request.UserId is null)
                throw new MarketBusinessRuleException("shift.open.waiterRequired",
                    "Izaberite konobara za kojeg se otvara smjena.");

            var waiter = await context.Users
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (waiter is null)
                throw new MarketNotFoundException($"Konobar (ID={request.UserId}) nije pronađen.");

            if (!waiter.IsEmployee || !waiter.IsEnabled)
                throw new MarketBusinessRuleException("shift.open.notWaiter",
                    $"Korisnik {waiter.Firstname} {waiter.Lastname} nije aktivan konobar.");

            userId = waiter.Id;
        }
        else if (appCurrentUser.IsEmployee)
        {
            userId = currentUserId;
        }
        else
        {
            throw new MarketForbiddenException("Nemate pravo otvaranja smjene.");
        }

        var cashRegister = await context.CashRegisters
            .FirstOrDefaultAsync(x => x.Id == request.CashRegisterId, cancellationToken);

        if (cashRegister is null)
            throw new MarketNotFoundException($"Kasa (ID={request.CashRegisterId}) nije pronađena.");

        if (!cashRegister.IsActive)
            throw new MarketBusinessRuleException("shift.open.cashRegisterInactive",
                $"Kasa {cashRegister.Name} nije aktivna.");

        bool userHasOpenShift = await context.Shifts
            .AnyAsync(x => x.UserId == userId && x.Status == ShiftStatusType.Open, cancellationToken);

        if (userHasOpenShift)
            throw new MarketBusinessRuleException("shift.open.userHasOpenShift",
                userId == currentUserId
                    ? "Već imate otvorenu smjenu. Zatvorite je prije otvaranja nove."
                    : "Izabrani konobar već ima otvorenu smjenu.");

        bool cashRegisterHasOpenShift = await context.Shifts
            .AnyAsync(x => x.CashRegisterId == cashRegister.Id && x.Status == ShiftStatusType.Open, cancellationToken);

        if (cashRegisterHasOpenShift)
            throw new MarketBusinessRuleException("shift.open.cashRegisterBusy",
                $"Na kasi {cashRegister.Name} je već otvorena smjena.");

        var shift = new ShiftEntity
        {
            CashRegisterId = cashRegister.Id,
            UserId = userId,
            OpenedAtUtc = timeProvider.GetUtcNow().UtcDateTime,
            OpeningCash = request.OpeningCash,
            Status = ShiftStatusType.Open
        };

        context.Shifts.Add(shift);
        await context.SaveChangesAsync(cancellationToken);

        return shift.Id;
    }
}
