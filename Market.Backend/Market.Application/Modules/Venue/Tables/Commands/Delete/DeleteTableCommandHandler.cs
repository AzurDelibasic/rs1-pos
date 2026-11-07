namespace Market.Application.Modules.Venue.Tables.Commands.Delete;

public sealed class DeleteTableCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser)
    : IRequestHandler<DeleteTableCommand, Unit>
{
    public async Task<Unit> Handle(DeleteTableCommand request, CancellationToken cancellationToken)
    {
        if (!appCurrentUser.IsAdmin && !appCurrentUser.IsManager)
            throw new MarketForbiddenException("Samo administrator i menadžer mogu brisati stolove.");

        var table = await context.Tables
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (table is null)
            throw new MarketNotFoundException($"Sto (ID={request.Id}) nije pronađen.");

        context.Tables.Remove(table);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
