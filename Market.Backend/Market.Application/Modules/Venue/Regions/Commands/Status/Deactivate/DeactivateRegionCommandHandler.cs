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

        if (entity.IsActive)
        {
            entity.IsActive = false;
            await ctx.SaveChangesAsync(ct);
        }

        return Unit.Value;
    }
}
