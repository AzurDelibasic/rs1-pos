namespace Market.Application.Modules.Settings.Countries.Commands.Update;

public sealed class UpdateCountryCommandHandler(IAppDbContext context)
    : IRequestHandler<UpdateCountryCommand, Unit>
{
    public async Task<Unit> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        var country = await context.Countries
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (country is null)
            throw new MarketNotFoundException($"Država (ID={request.Id}) nije pronađena.");

        var name = request.Name.Trim();
        var code = request.Code.Trim().ToUpperInvariant();

        bool nameExists = await context.Countries
            .AnyAsync(x => x.Id != request.Id && x.Name.ToLower() == name.ToLower(), cancellationToken);

        if (nameExists)
            throw new MarketConflictException($"Država sa nazivom {name} već postoji.");

        bool codeExists = await context.Countries
            .AnyAsync(x => x.Id != request.Id && x.Code == code, cancellationToken);

        if (codeExists)
            throw new MarketConflictException($"Država sa oznakom {code} već postoji.");

        country.Name = name;
        country.Code = code;
        country.VatRate = request.VatRate;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
