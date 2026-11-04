using Market.Application.Modules.Venue.Tables.Commands.Create;
using Market.Application.Modules.Venue.Tables.Commands.Delete;
using Market.Application.Modules.Venue.Tables.Commands.Status.Activate;
using Market.Application.Modules.Venue.Tables.Commands.Status.Deactivate;
using Market.Application.Modules.Venue.Tables.Commands.Update;
using Market.Application.Modules.Venue.Tables.Queries.GetById;
using Market.Application.Modules.Venue.Tables.Queries.List;
using Market.Shared.Constants;

namespace Market.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = PolicyNames.Staff)]
public class TablesController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = PolicyNames.Admin)]
    public async Task<ActionResult<int>> Create(CreateTableCommand command, CancellationToken ct)
    {
        int id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = PolicyNames.Admin)]
    public async Task Update(int id, UpdateTableCommand command, CancellationToken ct)
    {
        command.Id = id;
        await sender.Send(command, ct);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = PolicyNames.Admin)]
    public async Task Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteTableCommand { Id = id }, ct);
    }

    [HttpGet("{id:int}")]
    public async Task<GetTableByIdQueryDto> GetById(int id, CancellationToken ct)
    {
        return await sender.Send(new GetTableByIdQuery { Id = id }, ct);
    }

    [HttpGet]
    public async Task<PageResult<ListTablesQueryDto>> List([FromQuery] ListTablesQuery query, CancellationToken ct)
    {
        return await sender.Send(query, ct);
    }

    [HttpPut("{id:int}/activate")]
    [Authorize(Policy = PolicyNames.Admin)]
    public async Task Activate(int id, CancellationToken ct)
    {
        await sender.Send(new ActivateTableCommand { Id = id }, ct);
    }

    [HttpPut("{id:int}/deactivate")]
    [Authorize(Policy = PolicyNames.Admin)]
    public async Task Deactivate(int id, CancellationToken ct)
    {
        await sender.Send(new DeactivateTableCommand { Id = id }, ct);
    }
}
