using Market.Domain.Entities.Venue;

namespace Market.Application.Modules.Venue.Regions.Commands.Update;

public sealed class UpdateRegionCommandValidator : AbstractValidator<UpdateRegionCommand>
{
    public UpdateRegionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Naziv je obavezan.")
            .MaximumLength(RegionEntity.Constraints.NameMaxLength)
            .WithMessage($"Naziv može imati najviše {RegionEntity.Constraints.NameMaxLength} znakova.");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Redoslijed ne može biti negativan.");
    }
}