using Market.Domain.Entities.Shifts;

namespace Market.Application.Modules.Shifts.Shifts.Commands.Close;

public sealed class CloseShiftCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser, TimeProvider timeProvider)
    : IRequestHandler<CloseShiftCommand, Unit>
{
    public async Task<Unit> Handle(CloseShiftCommand request, CancellationToken cancellationToken)
    {
        var shift = await context.Shifts
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (shift is null)
            throw new MarketNotFoundException($"Smjena (ID={request.Id}) nije pronađena.");

        if (shift.UserId != appCurrentUser.UserId && !appCurrentUser.IsAdmin && !appCurrentUser.IsManager)
            throw new MarketForbiddenException("Možete zatvoriti samo svoju smjenu.");

        if (shift.Status != ShiftStatusType.Open)
            throw new MarketBusinessRuleException("shift.close.alreadyClosed", "Smjena je već zatvorena.");

        var expectedCash = shift.OpeningCash;

        shift.ExpectedCash = expectedCash;
        shift.CountedCash = request.CountedCash;
        shift.Difference = request.CountedCash - expectedCash;
        shift.ClosedAtUtc = timeProvider.GetUtcNow().UtcDateTime;
        shift.Status = ShiftStatusType.Closed;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
