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

        cashRegister.Name = name;
        cashRegister.IsActive = request.IsActive;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
