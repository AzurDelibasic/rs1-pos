namespace Market.Application.Modules.Venue.Tables.Commands.Status.Activate;

public sealed class ActivateTableCommandValidator : AbstractValidator<ActivateTableCommand>
{
    public ActivateTableCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
