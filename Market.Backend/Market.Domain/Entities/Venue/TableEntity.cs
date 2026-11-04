using Market.Domain.Common;

namespace Market.Domain.Entities.Venue;

public class TableEntity : BaseEntity
{
    public string Label { get; set; } = string.Empty;

    public int Seats { get; set; }

    public bool IsActive { get; set; } = true;

    public int RegionId { get; set; }

    public RegionEntity? Region { get; set; }

    public static class Constraints
    {
        public const int LabelMaxLength = 20;
        public const int SeatsMin = 1;
        public const int SeatsMax = 50;
    }
}
