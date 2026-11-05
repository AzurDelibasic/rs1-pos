using Market.Application.Modules.Shifts.CashRegisters.Commands.Create;
using Market.Application.Modules.Shifts.CashRegisters.Commands.Delete;
using Market.Application.Modules.Shifts.CashRegisters.Commands.Status.Activate;
using Market.Application.Modules.Shifts.CashRegisters.Commands.Status.Deactivate;
using Market.Application.Modules.Shifts.CashRegisters.Commands.Update;
using Market.Application.Modules.Shifts.CashRegisters.Queries.GetById;
using Market.Application.Modules.Shifts.CashRegisters.Queries.List;
using Market.Shared.Constants;

namespace Market.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = PolicyNames.Staff)]
public class CashRegistersController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = PolicyNames.Admin)]
    public async Task<ActionResult<int>> Create(CreateCashRegisterCommand command, CancellationToken ct)
    {
        int id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = PolicyNames.Admin)]
    public async Task Update(int id, UpdateCashRegisterCommand command, CancellationToken ct)
    {
        command.Id = id;
        await sender.Send(command, ct);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = PolicyNames.Admin)]
    public async Task Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteCashRegisterCommand { Id = id }, ct);
    }

    [HttpGet("{id:int}")]
    public async Task<GetCashRegisterByIdQueryDto> GetById(int id, CancellationToken ct)
    {
        return await sender.Send(new GetCashRegisterByIdQuery { Id = id }, ct);
    }

    [HttpGet]
    public async Task<PageResult<ListCashRegistersQueryDto>> List([FromQuery] ListCashRegistersQuery query, CancellationToken ct)
    {
        return await sender.Send(query, ct);
    }

    [HttpPut("{id:int}/activate")]
    [Authorize(Policy = PolicyNames.Admin)]
    public async Task Activate(int id, CancellationToken ct)
    {
        await sender.Send(new ActivateCashRegisterCommand { Id = id }, ct);
    }

    [HttpPut("{id:int}/deactivate")]
    [Authorize(Policy = PolicyNames.Admin)]
    public async Task Deactivate(int id, CancellationToken ct)
    {
        await sender.Send(new DeactivateCashRegisterCommand { Id = id }, ct);
    }
}
