namespace Market.Application.Modules.Shifts.CashRegisters.Commands.Status.Activate;

public sealed class ActivateCashRegisterCommandHandler(IAppDbContext ctx)
    : IRequestHandler<ActivateCashRegisterCommand, Unit>
{
    public async Task<Unit> Handle(ActivateCashRegisterCommand request, CancellationToken ct)
    {
        var entity = await ctx.CashRegisters
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"Kasa (ID={request.Id}) nije pronađena.");

        if (!entity.IsActive)
        {
            entity.IsActive = true;
            await ctx.SaveChangesAsync(ct);
        }

        return Unit.Value;
    }
}
