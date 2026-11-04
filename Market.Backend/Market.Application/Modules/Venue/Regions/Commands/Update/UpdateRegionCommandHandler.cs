namespace Market.Application.Modules.Venue.Regions.Commands.Update;

public sealed class UpdateRegionCommandHandler(IAppDbContext context)
    : IRequestHandler<UpdateRegionCommand, Unit>
{
    public async Task<Unit> Handle(UpdateRegionCommand request, CancellationToken cancellationToken)
    {
        var region = await context.Regions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (region is null)
            throw new MarketNotFoundException($"Regija (ID={request.Id}) nije pronađena.");

        var name = request.Name.Trim();

        bool exists = await context.Regions
            .AnyAsync(x => x.Id != request.Id && x.Name.ToLower() == name.ToLower(), cancellationToken);

        if (exists)
            throw new MarketConflictException($"Regija sa nazivom {name} već postoji.");

        if (region.IsActive && !request.IsActive)
        {
            var hasActiveTables = await context.Tables
                .AnyAsync(t => t.RegionId == region.Id && t.IsActive, cancellationToken);

            if (hasActiveTables)
                throw new MarketBusinessRuleException("region.deactivate.blocked.activeTables",
                    $"Regija {region.Name} se ne može deaktivirati jer sadrži aktivne stolove.");
        }

        region.Name = name;
        region.SortOrder = request.SortOrder;
        region.IsActive = request.IsActive;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}