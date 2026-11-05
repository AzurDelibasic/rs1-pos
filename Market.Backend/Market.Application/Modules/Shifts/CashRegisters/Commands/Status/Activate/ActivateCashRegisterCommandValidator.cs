namespace Market.Application.Modules.Shifts.CashRegisters.Commands.Status.Activate;

public sealed class ActivateCashRegisterCommandValidator : AbstractValidator<ActivateCashRegisterCommand>
{
    public ActivateCashRegisterCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
