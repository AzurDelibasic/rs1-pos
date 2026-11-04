namespace Market.Application.Modules.Venue.Tables.Commands.Update;

public sealed class UpdateTableCommandHandler(IAppDbContext ctx)
    : IRequestHandler<UpdateTableCommand, Unit>
{
    public async Task<Unit> Handle(UpdateTableCommand request, CancellationToken ct)
    {
        var entity = await ctx.Tables
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(ct);

        if (entity is null)
            throw new MarketNotFoundException($"Sto (ID={request.Id}) nije pronađen.");

        var region = await ctx.Regions
            .Where(x => x.Id == request.RegionId)
            .FirstOrDefaultAsync(ct);

        if (region is null)
        {
            throw new MarketNotFoundException($"Regija (ID={request.RegionId}) nije pronađena.");
        }

        if (!region.IsActive)
        {
            throw new MarketBusinessRuleException("table.region.inactive",
                $"Regija {region.Name} nije aktivna i ne može joj se dodijeliti sto.");
        }

        entity.Label = request.Label.Trim();
        entity.Seats = request.Seats;
        entity.RegionId = request.RegionId;
        entity.IsActive = request.IsActive;

        await ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
