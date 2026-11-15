using Market.Domain.Common;
using Market.Domain.Entities.Finance;

namespace Market.Domain.Entities.Settings;

public class AppSettingsEntity : BaseEntity
{
    public int CountryId { get; set; }

    public CountryEntity? Country { get; set; }

    public int DefaultCurrencyId { get; set; }

    public CurrencyEntity? DefaultCurrency { get; set; }

    public string BusinessName { get; set; } = string.Empty;

    public string? Address { get; set; }

    public string? TaxNumber { get; set; }

    public RoundingModeType RoundingMode { get; set; } = RoundingModeType.None;

    public int ReservationHoldMinutes { get; set; }

    public static class Constraints
    {
        public const int BusinessNameMaxLength = 100;
        public const int AddressMaxLength = 200;
        public const int TaxNumberMaxLength = 20;
        public const int ReservationHoldMinutesMin = 0;
        public const int ReservationHoldMinutesMax = 240;
    }
}
