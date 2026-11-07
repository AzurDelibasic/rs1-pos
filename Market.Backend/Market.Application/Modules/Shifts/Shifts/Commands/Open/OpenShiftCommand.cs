namespace Market.Application.Modules.Shifts.Shifts.Commands.Open;

public sealed class OpenShiftCommand : IRequest<int>
{
    public required int CashRegisterId { get; set; }

    public required decimal OpeningCash { get; set; }

    public int? UserId { get; set; }
}
