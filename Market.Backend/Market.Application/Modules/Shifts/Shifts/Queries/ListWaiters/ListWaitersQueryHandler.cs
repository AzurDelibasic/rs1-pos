using Market.Domain.Entities.Shifts;

namespace Market.Application.Modules.Shifts.Shifts.Queries.ListWaiters;

public sealed class ListWaitersQueryHandler(IAppDbContext ctx, IAppCurrentUser appCurrentUser)
    : IRequestHandler<ListWaitersQuery, PageResult<ListWaitersQueryDto>>
{
    public async Task<PageResult<ListWaitersQueryDto>> Handle(ListWaitersQuery request, CancellationToken ct)
    {
        if (!appCurrentUser.IsAdmin && !appCurrentUser.IsManager)
            throw new MarketForbiddenException("Samo administrator i menadžer mogu pregledati konobare.");

        var q = ctx.Users
            .AsNoTracking()
            .Where(x => x.IsEmployee && x.IsEnabled);

        if (!string.IsNullOrWhiteSpace(request.Search))
            q = q.Where(x => (x.Firstname + " " + x.Lastname).Contains(request.Search));

        var projected = q
            .OrderBy(x => x.Firstname)
            .ThenBy(x => x.Lastname)
            .ThenBy(x => x.Id)
            .Select(x => new ListWaitersQueryDto
            {
                Id = x.Id,
                FullName = x.Firstname + " " + x.Lastname,
                HasOpenShift = ctx.Shifts.Any(s => s.UserId == x.Id && s.Status == ShiftStatusType.Open)
            });

        return await PageResult<ListWaitersQueryDto>.FromQueryableAsync(projected, request.Paging, ct);
    }
}
