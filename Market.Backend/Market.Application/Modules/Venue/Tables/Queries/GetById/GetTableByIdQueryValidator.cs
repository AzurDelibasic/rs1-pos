namespace Market.Application.Modules.Venue.Tables.Queries.GetById;

public sealed class GetTableByIdQueryValidator : AbstractValidator<GetTableByIdQuery>
{
    public GetTableByIdQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id mora biti pozitivan broj.");
    }
}
