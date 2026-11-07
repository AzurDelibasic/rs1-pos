namespace Market.Application.Modules.Shifts.Shifts.Queries.List;

public sealed class ListShiftsQueryValidator : AbstractValidator<ListShiftsQuery>
{
    public ListShiftsQueryValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Nepoznat status smjene.");

        RuleFor(x => x.DateToUtc)
            .GreaterThanOrEqualTo(x => x.DateFromUtc)
            .When(x => x.DateFromUtc is not null && x.DateToUtc is not null)
            .WithMessage("Datum do ne može biti prije datuma od.");
    }
}
