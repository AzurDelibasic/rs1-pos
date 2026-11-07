using Market.Application.Modules.Shifts.Shifts.Commands.Close;
using Market.Application.Modules.Shifts.Shifts.Commands.Open;
using Market.Application.Modules.Shifts.Shifts.Queries.GetCurrent;
using Market.Application.Modules.Shifts.Shifts.Queries.List;
using Market.Application.Modules.Shifts.Shifts.Queries.ListWaiters;
using Market.Shared.Constants;

namespace Market.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = PolicyNames.Staff)]
public class ShiftsController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<int>> Open(OpenShiftCommand command, CancellationToken ct)
    {
        int id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetCurrent), null, new { id });
    }

    [HttpPut("{id:int}/close")]
    public async Task Close(int id, CloseShiftCommand command, CancellationToken ct)
    {
        command.Id = id;
        await sender.Send(command, ct);
    }

    [HttpGet("current")]
    public async Task<GetCurrentShiftQueryDto?> GetCurrent(CancellationToken ct)
    {
        return await sender.Send(new GetCurrentShiftQuery(), ct);
    }

    [HttpGet("waiters")]
    public async Task<PageResult<ListWaitersQueryDto>> ListWaiters([FromQuery] ListWaitersQuery query, CancellationToken ct)
    {
        return await sender.Send(query, ct);
    }

    [HttpGet]
    public async Task<PageResult<ListShiftsQueryDto>> List([FromQuery] ListShiftsQuery query, CancellationToken ct)
    {
        return await sender.Send(query, ct);
    }
}
