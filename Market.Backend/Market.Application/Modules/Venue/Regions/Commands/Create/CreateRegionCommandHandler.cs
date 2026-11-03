using Market.Domain.Entities.Venue;

namespace Market.Application.Modules.Venue.Regions.Commands.Create
{
    public sealed class CreateRegionCommandHandler(IAppDbContext context)
        : IRequestHandler<CreateRegionCommand, int>
    {
        public async Task<int> Handle(CreateRegionCommand request, CancellationToken cancellationToken)
        {
            var name = request.Name.Trim();

            bool exsist = await context.Regions
                .AnyAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken);

            if (exsist)
                throw new MarketConflictException($"Regija sa nazivom {name} već postoji.");

            var region = new RegionEntity
            {
                Name = name,
                SortOrder = request.SortOrder,
                IsActive = request.IsActive
            };

            context.Regions.Add(region);
            await context.SaveChangesAsync(cancellationToken);

            return region.Id;
        }
    }
}
