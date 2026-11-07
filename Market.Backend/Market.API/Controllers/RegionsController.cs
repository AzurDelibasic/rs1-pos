using Market.Application.Modules.Venue.Regions.Commands.Create;
using Market.Application.Modules.Venue.Regions.Commands.Delete;
using Market.Application.Modules.Venue.Regions.Commands.Status.Activate;
using Market.Application.Modules.Venue.Regions.Commands.Status.Deactivate;
using Market.Application.Modules.Venue.Regions.Commands.Update;
using Market.Application.Modules.Venue.Regions.Queries.GetById;
using Market.Application.Modules.Venue.Regions.Queries.List;
using Market.Shared.Constants;

namespace Market.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = PolicyNames.Staff)]
public class RegionsController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = PolicyNames.Management)]
    public async Task<ActionResult<int>> Create(CreateRegionCommand command, CancellationToken ct)
    {
        int id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = PolicyNames.Management)]
    public async Task Update(int id, UpdateRegionCommand command, CancellationToken ct)
    {
        command.Id = id;
        await sender.Send(command, ct);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = PolicyNames.Management)]
    public async Task Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteRegionCommand { Id = id }, ct);
    }

    [HttpGet("{id:int}")]
    public async Task<GetRegionByIdQueryDto> GetById(int id, CancellationToken ct)
    {
        return await sender.Send(new GetRegionByIdQuery { Id = id }, ct);
    }

    [HttpGet]
    public async Task<PageResult<ListRegionsQueryDto>> List([FromQuery] ListRegionsQuery query, CancellationToken ct)
    {
        return await sender.Send(query, ct);
    }

    [HttpPut("{id:int}/activate")]
    [Authorize(Policy = PolicyNames.Management)]
    public async Task Activate(int id, CancellationToken ct)
    {
        await sender.Send(new ActivateRegionCommand { Id = id }, ct);
    }

    [HttpPut("{id:int}/deactivate")]
    [Authorize(Policy = PolicyNames.Management)]
    public async Task Deactivate(int id, CancellationToken ct)
    {
        await sender.Send(new DeactivateRegionCommand { Id = id }, ct);
    }
}
