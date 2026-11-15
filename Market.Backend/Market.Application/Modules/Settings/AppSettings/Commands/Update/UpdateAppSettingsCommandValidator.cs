using Market.Domain.Entities.Settings;

namespace Market.Application.Modules.Settings.AppSettings.Commands.Update;

public sealed class UpdateAppSettingsCommandValidator : AbstractValidator<UpdateAppSettingsCommand>
{
    public UpdateAppSettingsCommandValidator()
    {
        RuleFor(x => x.BusinessName)
            .NotEmpty().WithMessage("Naziv objekta je obavezan.")
            .MaximumLength(AppSettingsEntity.Constraints.BusinessNameMaxLength)
            .WithMessage($"Naziv objekta može imati najviše {AppSettingsEntity.Constraints.BusinessNameMaxLength} znakova.");

        RuleFor(x => x.Address)
            .MaximumLength(AppSettingsEntity.Constraints.AddressMaxLength)
            .WithMessage($"Adresa može imati najviše {AppSettingsEntity.Constraints.AddressMaxLength} znakova.");

        RuleFor(x => x.TaxNumber)
            .MaximumLength(AppSettingsEntity.Constraints.TaxNumberMaxLength)
            .WithMessage($"PIB/JIB može imati najviše {AppSettingsEntity.Constraints.TaxNumberMaxLength} znakova.");

        RuleFor(x => x.CountryId)
            .GreaterThan(0).WithMessage("Država je obavezna.");

        RuleFor(x => x.DefaultCurrencyCode)
            .NotEmpty().WithMessage("Zadana valuta je obavezna.");

        RuleFor(x => x.RoundingMode)
            .IsInEnum().WithMessage("Nepoznat način zaokruživanja.");

        RuleFor(x => x.ReservationHoldMinutes)
            .InclusiveBetween(AppSettingsEntity.Constraints.ReservationHoldMinutesMin, AppSettingsEntity.Constraints.ReservationHoldMinutesMax)
            .WithMessage($"Vrijeme čekanja rezervacije mora biti između {AppSettingsEntity.Constraints.ReservationHoldMinutesMin} i {AppSettingsEntity.Constraints.ReservationHoldMinutesMax} minuta.");
    }
}
