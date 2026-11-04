using Market.Domain.Entities.Venue;

namespace Market.Application.Modules.Venue.Tables.Commands.Create;

public sealed class CreateTableCommandHandler(IAppDbContext ctx)
    : IRequestHandler<CreateTableCommand, int>
{
    public async Task<int> Handle(CreateTableCommand request, CancellationToken ct)
    {
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

        var table = new TableEntity
        {
            Label = request.Label.Trim(),
            Seats = request.Seats,
            RegionId = request.RegionId,
            IsActive = request.IsActive
        };

        ctx.Tables.Add(table);
        await ctx.SaveChangesAsync(ct);

        return table.Id;
    }
}
