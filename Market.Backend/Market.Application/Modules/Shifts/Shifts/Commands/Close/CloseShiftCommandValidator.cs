using Market.Domain.Entities.Shifts;

namespace Market.Application.Modules.Shifts.Shifts.Commands.Close;

public sealed class CloseShiftCommandValidator : AbstractValidator<CloseShiftCommand>
{
    public CloseShiftCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.CountedCash)
            .GreaterThanOrEqualTo(0).WithMessage("Izbrojano stanje ne može biti negativno.")
            .LessThanOrEqualTo(ShiftEntity.Constraints.CashMax)
            .WithMessage($"Izbrojano stanje može biti najviše {ShiftEntity.Constraints.CashMax}.");
    }
}
