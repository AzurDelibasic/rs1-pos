namespace Market.Application.Modules.Venue.Regions.Commands.Status.Activate;

public sealed class ActivateRegionCommandHandler(IAppDbContext ctx)
    : IRequestHandler<ActivateRegionCommand, Unit>
{
    public async Task<Unit> Handle(ActivateRegionCommand request, CancellationToken ct)
    {
        var entity = await ctx.Regions
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (entity is null)
            throw new MarketNotFoundException($"Regija (ID={request.Id}) nije pronađena.");

        if (!entity.IsActive)
        {
            entity.IsActive = true;
            await ctx.SaveChangesAsync(ct);
        }

        return Unit.Value;
    }
}
