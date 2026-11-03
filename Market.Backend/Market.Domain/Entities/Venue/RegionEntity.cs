using Market.Domain.Common;

namespace Market.Domain.Entities.Venue
{
    public class RegionEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public static class Constraints
        {
            public const int NameMaxLength = 50;
        }
    }
}
