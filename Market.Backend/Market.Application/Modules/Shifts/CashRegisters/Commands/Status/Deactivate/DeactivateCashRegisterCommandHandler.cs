using Market.Domain.Entities.Shifts;

namespace Market.Application.Modules.Shifts.CashRegisters.Commands.Status.Deactivate;

public sealed class DeactivateCashRegisterCommandHandler(IAppDbContext ctx)
    : IRequestHandler<DeactivateCashRegisterCommand, Unit>
{
    public async Task<Unit> Handle(DeactivateCashRegisterCommand request, CancellationToken ct)
    {
        var entity = await ctx.CashRegisters
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"Kasa (ID={request.Id}) nije pronađena.");

        if (!entity.IsActive) return Unit.Value;

        var hasOpenShift = await ctx.Shifts
            .AnyAsync(s => s.CashRegisterId == entity.Id && s.Status == ShiftStatusType.Open, ct);

        if (hasOpenShift)
        {
            throw new MarketBusinessRuleException("cashRegister.deactivate.blocked.openShift",
                $"Kasa {entity.Name} se ne može deaktivirati jer je na njoj otvorena smjena.");
        }

        entity.IsActive = false;

        await ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
