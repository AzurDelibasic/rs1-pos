namespace Market.Application.Modules.Venue.Regions.Commands.Status.Deactivate;

public sealed class DeactivateRegionCommandValidator : AbstractValidator<DeactivateRegionCommand>
{
    public DeactivateRegionCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
