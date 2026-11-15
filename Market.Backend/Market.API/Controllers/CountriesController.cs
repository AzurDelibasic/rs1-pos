using Market.Application.Modules.Settings.Countries.Commands.Create;
using Market.Application.Modules.Settings.Countries.Commands.Delete;
using Market.Application.Modules.Settings.Countries.Commands.Update;
using Market.Application.Modules.Settings.Countries.Queries.GetById;
using Market.Application.Modules.Settings.Countries.Queries.List;
using Market.Shared.Constants;

namespace Market.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = PolicyNames.Staff)]
public class CountriesController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = PolicyNames.Admin)]
    public async Task<ActionResult<int>> Create(CreateCountryCommand command, CancellationToken ct)
    {
        int id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = PolicyNames.Admin)]
    public async Task Update(int id, UpdateCountryCommand command, CancellationToken ct)
    {
        command.Id = id;
        await sender.Send(command, ct);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = PolicyNames.Admin)]
    public async Task Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteCountryCommand { Id = id }, ct);
    }

    [HttpGet("{id:int}")]
    public async Task<GetCountryByIdQueryDto> GetById(int id, CancellationToken ct)
    {
        return await sender.Send(new GetCountryByIdQuery { Id = id }, ct);
    }

    [HttpGet]
    public async Task<PageResult<ListCountriesQueryDto>> List([FromQuery] ListCountriesQuery query, CancellationToken ct)
    {
        return await sender.Send(query, ct);
    }
}
