namespace Market.Application.Modules.Shifts.CashRegisters.Commands.Update;

public sealed class UpdateCashRegisterCommand : IRequest<Unit>
{
    [JsonIgnore]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required bool IsActive { get; set; }
}
