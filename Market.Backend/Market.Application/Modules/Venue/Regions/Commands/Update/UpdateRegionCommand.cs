namespace Market.Application.Modules.Venue.Regions.Commands.Update
{
    public sealed class UpdateRegionCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; }

        public required string Name { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
