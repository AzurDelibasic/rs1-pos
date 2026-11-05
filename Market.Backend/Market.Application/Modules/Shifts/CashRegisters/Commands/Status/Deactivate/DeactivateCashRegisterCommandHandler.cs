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

        if (entity.IsActive)
        {
            entity.IsActive = false;
            await ctx.SaveChangesAsync(ct);
        }

        return Unit.Value;
    }
}
