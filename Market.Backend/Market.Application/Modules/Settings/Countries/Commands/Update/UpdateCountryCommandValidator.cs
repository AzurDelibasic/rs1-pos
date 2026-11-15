using Market.Domain.Entities.Settings;

namespace Market.Application.Modules.Settings.Countries.Commands.Update;

public sealed class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
{
    public UpdateCountryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Naziv je obavezan.")
            .MaximumLength(CountryEntity.Constraints.NameMaxLength)
            .WithMessage($"Naziv može imati najviše {CountryEntity.Constraints.NameMaxLength} znakova.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Oznaka je obavezna.")
            .Matches("^[A-Za-z]{2}$").WithMessage("Oznaka mora imati tačno 2 slova (npr. BA).");

        RuleFor(x => x.VatRate)
            .InclusiveBetween(CountryEntity.Constraints.VatRateMin, CountryEntity.Constraints.VatRateMax)
            .WithMessage($"Stopa PDV-a mora biti između {CountryEntity.Constraints.VatRateMin} i {CountryEntity.Constraints.VatRateMax}.");
    }
}
