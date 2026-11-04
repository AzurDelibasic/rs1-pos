namespace Market.Application.Modules.Venue.Tables.Commands.Status.Activate;

public sealed class ActivateTableCommandHandler(IAppDbContext ctx)
    : IRequestHandler<ActivateTableCommand, Unit>
{
    public async Task<Unit> Handle(ActivateTableCommand request, CancellationToken ct)
    {
        var entity = await ctx.Tables
            .Include(x => x.Region)
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"Sto (ID={request.Id}) nije pronađen.");

        if (entity.IsActive)
            return Unit.Value;

        if (!entity.Region!.IsActive)
        {
            throw new MarketBusinessRuleException("table.region.inactive",
                $"Sto {entity.Label} se ne može aktivirati jer regija {entity.Region.Name} nije aktivna.");
        }

        entity.IsActive = true;
        await ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
