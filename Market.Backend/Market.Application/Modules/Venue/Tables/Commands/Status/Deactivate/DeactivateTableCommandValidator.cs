namespace Market.Application.Modules.Venue.Tables.Commands.Status.Deactivate;

public sealed class DeactivateTableCommandValidator : AbstractValidator<DeactivateTableCommand>
{
    public DeactivateTableCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
