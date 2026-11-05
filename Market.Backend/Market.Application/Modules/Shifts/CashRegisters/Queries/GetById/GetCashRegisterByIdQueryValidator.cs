namespace Market.Application.Modules.Shifts.CashRegisters.Queries.GetById;

public sealed class GetCashRegisterByIdQueryValidator : AbstractValidator<GetCashRegisterByIdQuery>
{
    public GetCashRegisterByIdQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id mora biti pozitivan broj.");
    }
}
