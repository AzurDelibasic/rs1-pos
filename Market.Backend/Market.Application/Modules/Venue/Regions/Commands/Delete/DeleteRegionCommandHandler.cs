namespace Market.Application.Modules.Venue.Regions.Commands.Delete;

public sealed class DeleteRegionCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser)
    : IRequestHandler<DeleteRegionCommand, Unit>
{
    public async Task<Unit> Handle(DeleteRegionCommand request, CancellationToken cancellationToken)
    {
        if (!appCurrentUser.IsAdmin)
            throw new MarketForbiddenException("Samo administrator može brisati regije.");

        var region = await context.Regions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (region is null)
            throw new MarketNotFoundException($"Regija (ID={request.Id}) nije pronađena.");

        context.Regions.Remove(region);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
