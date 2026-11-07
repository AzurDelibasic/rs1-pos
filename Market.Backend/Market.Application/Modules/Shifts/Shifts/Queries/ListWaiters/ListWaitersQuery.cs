namespace Market.Application.Modules.Shifts.Shifts.Queries.ListWaiters;

public sealed class ListWaitersQuery : BasePagedQuery<ListWaitersQueryDto>
{
    public string? Search { get; set; }
}
