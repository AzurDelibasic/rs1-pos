using Market.Domain.Entities.Shifts;

namespace Market.Application.Modules.Shifts.Shifts.Queries.List;

public sealed class ListShiftsQueryDto
{
    public required int Id { get; init; }
    public required string CashRegisterName { get; init; }
    public required string UserFullName { get; init; }
    public required DateTime OpenedAtUtc { get; init; }
    public DateTime? ClosedAtUtc { get; init; }
    public required decimal OpeningCash { get; init; }
    public decimal? ExpectedCash { get; init; }
    public decimal? CountedCash { get; init; }
    public decimal? Difference { get; init; }
    public required ShiftStatusType Status { get; init; }
}
