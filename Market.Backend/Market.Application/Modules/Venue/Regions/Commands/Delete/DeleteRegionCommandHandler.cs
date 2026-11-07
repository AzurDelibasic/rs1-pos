namespace Market.Application.Modules.Venue.Regions.Commands.Delete;

public sealed class DeleteRegionCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser)
    : IRequestHandler<DeleteRegionCommand, Unit>
{
    public async Task<Unit> Handle(DeleteRegionCommand request, CancellationToken cancellationToken)
    {
        if (!appCurrentUser.IsAdmin && !appCurrentUser.IsManager)
            throw new MarketForbiddenException("Samo administrator i menadžer mogu brisati regije.");

        var region = await context.Regions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (region is null)
            throw new MarketNotFoundException($"Regija (ID={request.Id}) nije pronađena.");

        var hasTables = await context.Tables
            .AnyAsync(t => t.RegionId == region.Id, cancellationToken);

        if (hasTables)
            throw new MarketConflictException(
                $"Regija {region.Name} se ne može obrisati jer sadrži stolove. " +
                "Premjestite stolove u drugu regiju ili deaktivirajte regiju.");

        context.Regions.Remove(region);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
