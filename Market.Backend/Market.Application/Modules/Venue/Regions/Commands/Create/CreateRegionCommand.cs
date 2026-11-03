namespace Market.Application.Modules.Venue.Regions.Commands.Create
{
    public sealed class CreateRegionCommand : IRequest<int>
    {
        public required string Name { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
