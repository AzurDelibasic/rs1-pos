namespace Market.Application.Modules.Venue.Tables.Commands.Status.Deactivate;

public sealed class DeactivateTableCommandHandler(IAppDbContext ctx)
    : IRequestHandler<DeactivateTableCommand, Unit>
{
    public async Task<Unit> Handle(DeactivateTableCommand request, CancellationToken ct)
    {
        var entity = await ctx.Tables
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"Sto (ID={request.Id}) nije pronađen.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            await ctx.SaveChangesAsync(ct);
        }

        return Unit.Value;
    }
}
