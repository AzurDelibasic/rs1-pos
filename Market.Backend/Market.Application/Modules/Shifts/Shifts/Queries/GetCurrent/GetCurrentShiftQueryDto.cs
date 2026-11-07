namespace Market.Application.Modules.Shifts.Shifts.Queries.GetCurrent;

public sealed class GetCurrentShiftQueryDto
{
    public required int Id { get; init; }
    public required int CashRegisterId { get; init; }
    public required string CashRegisterName { get; init; }
    public required DateTime OpenedAtUtc { get; init; }
    public required decimal OpeningCash { get; init; }
    public required decimal ExpectedCash { get; init; }
}
