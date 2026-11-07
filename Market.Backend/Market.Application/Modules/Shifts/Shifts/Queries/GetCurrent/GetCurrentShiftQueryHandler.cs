using Market.Domain.Entities.Shifts;

namespace Market.Application.Modules.Shifts.Shifts.Queries.GetCurrent;

public sealed class GetCurrentShiftQueryHandler(IAppDbContext context, IAppCurrentUser appCurrentUser)
    : IRequestHandler<GetCurrentShiftQuery, GetCurrentShiftQueryDto?>
{
    public async Task<GetCurrentShiftQueryDto?> Handle(GetCurrentShiftQuery request, CancellationToken cancellationToken)
    {
        var userId = appCurrentUser.UserId
            ?? throw new MarketUnauthorizedException("Korisnik nije prijavljen.");

        return await context.Shifts
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Status == ShiftStatusType.Open)
            .Select(x => new GetCurrentShiftQueryDto
            {
                Id = x.Id,
                CashRegisterId = x.CashRegisterId,
                CashRegisterName = x.CashRegister!.Name,
                OpenedAtUtc = x.OpenedAtUtc,
                OpeningCash = x.OpeningCash,
                ExpectedCash = x.OpeningCash
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
