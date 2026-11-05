namespace Market.Application.Modules.Shifts.CashRegisters.Commands.Create;

public sealed class CreateCashRegisterCommand : IRequest<int>
{
    public required string Name { get; set; }

    public bool IsActive { get; set; } = true;
}
