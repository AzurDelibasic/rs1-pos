using Market.Application.Modules.Venue.FloorPlan.Queries.GetFloorPlan;
using Market.Shared.Constants;

namespace Market.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = PolicyNames.Staff)]
public class FloorPlanController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<GetFloorPlanQueryDto> Get(CancellationToken ct)
    {
        return await sender.Send(new GetFloorPlanQuery(), ct);
    }
}
