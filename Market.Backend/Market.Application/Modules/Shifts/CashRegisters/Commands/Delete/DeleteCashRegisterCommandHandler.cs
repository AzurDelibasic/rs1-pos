namespace Market.Application.Modules.Shifts.CashRegisters.Commands.Delete;

public sealed class DeleteCashRegisterCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser)
    : IRequestHandler<DeleteCashRegisterCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCashRegisterCommand request, CancellationToken cancellationToken)
    {
        if (!appCurrentUser.IsAdmin && !appCurrentUser.IsManager)
            throw new MarketForbiddenException("Samo administrator i menadžer mogu brisati kase.");

        var cashRegister = await context.CashRegisters
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (cashRegister is null)
            throw new MarketNotFoundException($"Kasa (ID={request.Id}) nije pronađena.");

        var hasShifts = await context.Shifts
            .AnyAsync(s => s.CashRegisterId == cashRegister.Id, cancellationToken);

        if (hasShifts)
            throw new MarketConflictException(
                $"Kasa {cashRegister.Name} se ne može obrisati jer ima evidentirane smjene. " +
                "Deaktivirajte kasu umjesto brisanja.");

        context.CashRegisters.Remove(cashRegister);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
