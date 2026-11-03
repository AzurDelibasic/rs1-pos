namespace Market.Application.Modules.Venue.Regions.Commands.Update;

public sealed class UpdateRegionCommandHandler(IAppDbContext context)
    : IRequestHandler<UpdateRegionCommand, Unit>
{
    public async Task<Unit> Handle(UpdateRegionCommand request, CancellationToken cancellationToken)
    {
        var region = await context.Regions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (region is null)
            throw new MarketNotFoundException("Regija nije pronađena.");

        var name = request.Name.Trim();

        bool exists = await context.Regions
            .AnyAsync(x => x.Id != request.Id && x.Name.ToLower() == name.ToLower(), cancellationToken);

        if (exists)
            throw new MarketConflictException($"Regija sa nazivom {name} već postoji.");

        region.Name = name;
        region.SortOrder = request.SortOrder;
        region.IsActive = request.IsActive;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}