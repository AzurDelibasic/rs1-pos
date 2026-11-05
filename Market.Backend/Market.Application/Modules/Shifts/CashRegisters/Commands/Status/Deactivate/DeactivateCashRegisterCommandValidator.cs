namespace Market.Application.Modules.Shifts.CashRegisters.Commands.Status.Deactivate;

public sealed class DeactivateCashRegisterCommandValidator : AbstractValidator<DeactivateCashRegisterCommand>
{
    public DeactivateCashRegisterCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
