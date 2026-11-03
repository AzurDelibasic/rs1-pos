namespace Market.Application.Modules.Venue.Regions.Queries.GetById;

public sealed class GetRegionByIdQueryValidator : AbstractValidator<GetRegionByIdQuery>
{
    public GetRegionByIdQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id mora biti pozitivan broj.");
    }
}
