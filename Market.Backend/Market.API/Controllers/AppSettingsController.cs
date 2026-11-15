using Market.Application.Modules.Settings.AppSettings.Commands.Update;
using Market.Application.Modules.Settings.AppSettings.Queries.Get;
using Market.Shared.Constants;

namespace Market.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = PolicyNames.Staff)]
public class AppSettingsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<GetAppSettingsQueryDto> Get(CancellationToken ct)
    {
        return await sender.Send(new GetAppSettingsQuery(), ct);
    }

    [HttpPut]
    [Authorize(Policy = PolicyNames.Admin)]
    public async Task Update(UpdateAppSettingsCommand command, CancellationToken ct)
    {
        await sender.Send(command, ct);
    }
}
