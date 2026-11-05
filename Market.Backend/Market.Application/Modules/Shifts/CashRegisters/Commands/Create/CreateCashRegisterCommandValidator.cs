using Market.Domain.Entities.Shifts;

namespace Market.Application.Modules.Shifts.CashRegisters.Commands.Create;

public sealed class CreateCashRegisterCommandValidator : AbstractValidator<CreateCashRegisterCommand>
{
    public CreateCashRegisterCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Naziv je obavezan.")
            .MaximumLength(CashRegisterEntity.Constraints.NameMaxLength)
            .WithMessage($"Naziv može imati najviše {CashRegisterEntity.Constraints.NameMaxLength} znakova.");
    }
}
