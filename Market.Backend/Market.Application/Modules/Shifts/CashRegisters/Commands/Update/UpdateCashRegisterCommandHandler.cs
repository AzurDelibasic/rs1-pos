using Market.Domain.Entities.Shifts;

namespace Market.Application.Modules.Shifts.CashRegisters.Commands.Update;

public sealed class UpdateCashRegisterCommandHandler(IAppDbContext context)
    : IRequestHandler<UpdateCashRegisterCommand, Unit>
{
    public async Task<Unit> Handle(UpdateCashRegisterCommand request, CancellationToken cancellationToken)
    {
        var cashRegister = await context.CashRegisters
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (cashRegister is null)
            throw new MarketNotFoundException($"Kasa (ID={request.Id}) nije pronađena.");

        var name = request.Name.Trim();

        bool exists = await context.CashRegisters
            .AnyAsync(x => x.Id != request.Id && x.Name.ToLower() == name.ToLower(), cancellationToken);

        if (exists)
            throw new MarketConflictException($"Kasa sa nazivom {name} već postoji.");

        if (cashRegister.IsActive && !request.IsActive)
        {
            var hasOpenShift = await context.Shifts
                .AnyAsync(s => s.CashRegisterId == cashRegister.Id && s.Status == ShiftStatusType.Open, cancellationToken);

            if (hasOpenShift)
                throw new MarketBusinessRuleException("cashRegister.deactivate.blocked.openShift",
                    $"Kasa {cashRegister.Name} se ne može deaktivirati jer je na njoj otvorena smjena.");
        }

        cashRegister.Name = name;
        cashRegister.IsActive = request.IsActive;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
