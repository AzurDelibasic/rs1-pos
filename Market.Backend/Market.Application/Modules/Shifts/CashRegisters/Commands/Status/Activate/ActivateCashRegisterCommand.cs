namespace Market.Application.Modules.Shifts.CashRegisters.Commands.Status.Activate;

public sealed class ActivateCashRegisterCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
