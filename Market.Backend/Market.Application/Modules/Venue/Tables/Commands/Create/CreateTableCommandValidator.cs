using Market.Domain.Entities.Venue;

namespace Market.Application.Modules.Venue.Tables.Commands.Create;

public sealed class CreateTableCommandValidator : AbstractValidator<CreateTableCommand>
{
    public CreateTableCommandValidator(IAppDbContext ctx)
    {
        RuleFor(x => x.Label)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Oznaka stola je obavezna.")
            .MaximumLength(TableEntity.Constraints.LabelMaxLength).WithMessage($"Oznaka može imati najviše {TableEntity.Constraints.LabelMaxLength} znakova.")
            .MustAsync(async (command, label, ct) =>
            {
                var normalized = label.Trim().ToLower();
                return !await ctx.Tables.AnyAsync(t => t.RegionId == command.RegionId && t.Label.ToLower() == normalized, ct);
            })
            .WithMessage("Sto sa ovom oznakom već postoji u izabranoj regiji.");

        RuleFor(x => x.Seats)
            .InclusiveBetween(TableEntity.Constraints.SeatsMin, TableEntity.Constraints.SeatsMax)
            .WithMessage($"Broj mjesta mora biti između {TableEntity.Constraints.SeatsMin} i {TableEntity.Constraints.SeatsMax}.");

        RuleFor(x => x.RegionId)
            .GreaterThan(0).WithMessage("Regija je obavezna.");
    }
}
