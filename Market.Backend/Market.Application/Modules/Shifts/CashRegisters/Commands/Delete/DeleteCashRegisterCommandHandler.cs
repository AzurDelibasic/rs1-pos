namespace Market.Application.Modules.Shifts.CashRegisters.Commands.Delete;

public sealed class DeleteCashRegisterCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser)
    : IRequestHandler<DeleteCashRegisterCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCashRegisterCommand request, CancellationToken cancellationToken)
    {
        if (!appCurrentUser.IsAdmin)
            throw new MarketForbiddenException("Samo administrator može brisati kase.");

        var cashRegister = await context.CashRegisters
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (cashRegister is null)
            throw new MarketNotFoundException($"Kasa (ID={request.Id}) nije pronađena.");

        context.CashRegisters.Remove(cashRegister);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
