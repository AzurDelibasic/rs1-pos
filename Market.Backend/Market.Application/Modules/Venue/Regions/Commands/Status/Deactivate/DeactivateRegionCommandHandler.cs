namespace Market.Application.Modules.Venue.Regions.Commands.Status.Deactivate;

public sealed class DeactivateRegionCommandHandler(IAppDbContext ctx)
    : IRequestHandler<DeactivateRegionCommand, Unit>
{
    public async Task<Unit> Handle(DeactivateRegionCommand request, CancellationToken ct)
    {
        var entity = await ctx.Regions
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"Regija (ID={request.Id}) nije pronađena.");

        if (!entity.IsActive) return Unit.Value;

        var hasActiveTables = await ctx.Tables
            .AnyAsync(t => t.RegionId == entity.Id && t.IsActive, ct);

        if (hasActiveTables)
        {
            throw new MarketBusinessRuleException("region.deactivate.blocked.activeTables",
                $"Regija {entity.Name} se ne može deaktivirati jer sadrži aktivne stolove.");
        }

        entity.IsActive = false;

        await ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
