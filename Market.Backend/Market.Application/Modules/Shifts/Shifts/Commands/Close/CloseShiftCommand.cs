namespace Market.Application.Modules.Shifts.Shifts.Commands.Close;

public sealed class CloseShiftCommand : IRequest<Unit>
{
    [JsonIgnore]
    public int Id { get; set; }

    public required decimal CountedCash { get; set; }
}
