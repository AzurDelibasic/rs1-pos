namespace Market.Application.Modules.Shifts.CashRegisters.Commands.Delete;

public sealed class DeleteCashRegisterCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
