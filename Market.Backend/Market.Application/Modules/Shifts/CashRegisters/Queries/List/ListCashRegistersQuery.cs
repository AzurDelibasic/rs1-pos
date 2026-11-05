namespace Market.Application.Modules.Shifts.CashRegisters.Queries.List;

public sealed class ListCashRegistersQuery : BasePagedQuery<ListCashRegistersQueryDto>
{
    public string? Search { get; set; }
    public bool? OnlyActive { get; set; }
}
