namespace Market.Application.Modules.Shifts.CashRegisters.Queries.List;

public sealed class ListCashRegistersQueryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required bool IsActive { get; init; }
}
