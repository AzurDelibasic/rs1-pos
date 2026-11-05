namespace Market.Application.Modules.Shifts.CashRegisters.Commands.Status.Deactivate;

public sealed class DeactivateCashRegisterCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
