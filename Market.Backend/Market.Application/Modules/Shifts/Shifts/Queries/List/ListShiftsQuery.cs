using Market.Domain.Entities.Shifts;

namespace Market.Application.Modules.Shifts.Shifts.Queries.List;

public sealed class ListShiftsQuery : BasePagedQuery<ListShiftsQueryDto>
{
    public string? Search { get; set; }
    public int? CashRegisterId { get; set; }
    public ShiftStatusType? Status { get; set; }
    public DateTime? DateFromUtc { get; set; }
    public DateTime? DateToUtc { get; set; }
}
