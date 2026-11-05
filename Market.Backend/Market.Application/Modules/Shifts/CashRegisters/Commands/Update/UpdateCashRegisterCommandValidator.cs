using Market.Domain.Entities.Shifts;

namespace Market.Application.Modules.Shifts.CashRegisters.Commands.Update;

public sealed class UpdateCashRegisterCommandValidator : AbstractValidator<UpdateCashRegisterCommand>
{
    public UpdateCashRegisterCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Naziv je obavezan.")
            .MaximumLength(CashRegisterEntity.Constraints.NameMaxLength)
            .WithMessage($"Naziv može imati najviše {CashRegisterEntity.Constraints.NameMaxLength} znakova.");
    }
}
