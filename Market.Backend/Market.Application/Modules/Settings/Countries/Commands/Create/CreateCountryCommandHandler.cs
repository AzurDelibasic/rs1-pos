using Market.Domain.Entities.Settings;

namespace Market.Application.Modules.Settings.Countries.Commands.Create;

public sealed class CreateCountryCommandHandler(IAppDbContext context)
    : IRequestHandler<CreateCountryCommand, int>
{
    public async Task<int> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();
        var code = request.Code.Trim().ToUpperInvariant();

        bool nameExists = await context.Countries
            .AnyAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken);

        if (nameExists)
            throw new MarketConflictException($"Država sa nazivom {name} već postoji.");

        bool codeExists = await context.Countries
            .AnyAsync(x => x.Code == code, cancellationToken);

        if (codeExists)
            throw new MarketConflictException($"Država sa oznakom {code} već postoji.");

        var country = new CountryEntity
        {
            Name = name,
            Code = code,
            VatRate = request.VatRate
        };

        context.Countries.Add(country);
        await context.SaveChangesAsync(cancellationToken);

        return country.Id;
    }
}
