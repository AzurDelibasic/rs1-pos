namespace Market.Application.Modules.Venue.Regions.Commands.Delete;

public sealed class DeleteRegionCommandHandler(IAppDbContext context)
    : IRequestHandler<DeleteRegionCommand, Unit>
{
    public async Task<Unit> Handle(DeleteRegionCommand request, CancellationToken cancellationToken)
    {
        var region = await context.Regions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (region is null)
            throw new MarketNotFoundException("Regija nije pronađena.");

        context.Regions.Remove(region);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}