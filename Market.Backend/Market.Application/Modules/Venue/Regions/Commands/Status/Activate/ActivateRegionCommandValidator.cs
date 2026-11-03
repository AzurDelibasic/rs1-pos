namespace Market.Application.Modules.Venue.Regions.Commands.Status.Activate;

public sealed class ActivateRegionCommandValidator : AbstractValidator<ActivateRegionCommand>
{
    public ActivateRegionCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
