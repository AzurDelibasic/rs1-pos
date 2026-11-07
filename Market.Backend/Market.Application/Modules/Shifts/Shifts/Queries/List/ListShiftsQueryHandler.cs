using Market.Domain.Entities.Shifts;

namespace Market.Application.Modules.Shifts.Shifts.Queries.List;

public sealed class ListShiftsQueryHandler(IAppDbContext ctx, IAppCurrentUser appCurrentUser)
    : IRequestHandler<ListShiftsQuery, PageResult<ListShiftsQueryDto>>
{
    public async Task<PageResult<ListShiftsQueryDto>> Handle(ListShiftsQuery request, CancellationToken ct)
    {
        if (!appCurrentUser.IsAdmin && !appCurrentUser.IsManager)
            throw new MarketForbiddenException("Samo administrator i menadžer mogu pregledati smjene.");

        var q = ctx.Shifts.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            q = q.Where(x => (x.User!.Firstname + " " + x.User.Lastname).Contains(request.Search));

        if (request.CashRegisterId is not null)
            q = q.Where(x => x.CashRegisterId == request.CashRegisterId);

        if (request.Status is not null)
            q = q.Where(x => x.Status == request.Status);

        if (request.DateFromUtc is not null)
        {
            var from = request.DateFromUtc.Value.AsUtc().Date;
            q = q.Where(x => x.OpenedAtUtc >= from);
        }

        if (request.DateToUtc is not null)
        {
            var to = request.DateToUtc.Value.AsUtc().Date.AddDays(1);
            q = q.Where(x => x.OpenedAtUtc < to);
        }

        var projected = q
            .OrderByDescending(x => x.OpenedAtUtc)
            .ThenByDescending(x => x.Id)
            .Select(x => new ListShiftsQueryDto
            {
                Id = x.Id,
                CashRegisterName = x.CashRegister!.Name,
                UserFullName = x.User!.Firstname + " " + x.User.Lastname,
                OpenedAtUtc = x.OpenedAtUtc,
                ClosedAtUtc = x.ClosedAtUtc,
                OpeningCash = x.OpeningCash,
                ExpectedCash = x.Status == ShiftStatusType.Open ? x.OpeningCash : x.ExpectedCash,
                CountedCash = x.CountedCash,
                Difference = x.Difference,
                Status = x.Status
            });

        return await PageResult<ListShiftsQueryDto>.FromQueryableAsync(projected, request.Paging, ct);
    }
}
