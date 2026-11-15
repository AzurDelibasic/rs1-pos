namespace Market.Application.Modules.Settings.Countries.Commands.Delete;

public sealed class DeleteCountryCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser)
    : IRequestHandler<DeleteCountryCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        if (!appCurrentUser.IsAdmin)
            throw new MarketForbiddenException("Samo administrator može brisati države.");

        var country = await context.Countries
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (country is null)
            throw new MarketNotFoundException($"Država (ID={request.Id}) nije pronađena.");

        var isSelectedInSettings = await context.AppSettings
            .AnyAsync(s => s.CountryId == country.Id, cancellationToken);

        if (isSelectedInSettings)
            throw new MarketConflictException(
                $"Država {country.Name} je izabrana u postavkama aplikacije i ne može se obrisati.");

        context.Countries.Remove(country);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
