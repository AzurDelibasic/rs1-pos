using Market.Domain.Common;

namespace Market.Domain.Entities.Settings;

public class CountryEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public decimal VatRate { get; set; }

    public static class Constraints
    {
        public const int NameMaxLength = 100;
        public const int CodeLength = 2;
        public const int VatRatePrecision = 5;
        public const int VatRateScale = 2;
        public const decimal VatRateMin = 0m;
        public const decimal VatRateMax = 100m;
    }
}
