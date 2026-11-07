namespace Market.Application.Modules.Shifts.Shifts.Queries.ListWaiters;

public sealed class ListWaitersQueryDto
{
    public required int Id { get; init; }
    public required string FullName { get; init; }
    public required bool HasOpenShift { get; init; }
}
