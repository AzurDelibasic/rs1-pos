using Market.Domain.Entities.Venue;

namespace Market.Application.Modules.Venue.Regions.Commands.Create
{
    public sealed class CreateRegionCommandValidator : AbstractValidator<CreateRegionCommand>
    {
        public CreateRegionCommandValidator() {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Naziv je obavezan.")
                .MaximumLength(RegionEntity.Constraints.NameMaxLength)
                .WithMessage($"Naziv može imati najvište {RegionEntity.Constraints.NameMaxLength} znakova.");

            RuleFor(x => x.SortOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Redoslijed ne može biti negativan.");
        }
    }
}
