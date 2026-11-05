namespace Market.Application.Modules.Shifts.CashRegisters.Queries.GetById;

public sealed class GetCashRegisterByIdQueryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required bool IsActive { get; init; }
}
