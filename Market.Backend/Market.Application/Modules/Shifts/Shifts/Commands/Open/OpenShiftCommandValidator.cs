using Market.Domain.Entities.Shifts;

namespace Market.Application.Modules.Shifts.Shifts.Commands.Open;

public sealed class OpenShiftCommandValidator : AbstractValidator<OpenShiftCommand>
{
    public OpenShiftCommandValidator()
    {
        RuleFor(x => x.CashRegisterId)
            .GreaterThan(0).WithMessage("Kasa je obavezna.");

        RuleFor(x => x.UserId)
            .GreaterThan(0).When(x => x.UserId is not null).WithMessage("Konobar nije ispravan.");

        RuleFor(x => x.OpeningCash)
            .GreaterThanOrEqualTo(0).WithMessage("Početno stanje ne može biti negativno.")
            .LessThanOrEqualTo(ShiftEntity.Constraints.CashMax)
            .WithMessage($"Početno stanje može biti najviše {ShiftEntity.Constraints.CashMax}.");
    }
}
