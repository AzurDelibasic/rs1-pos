using Market.Domain.Entities.Settings;

namespace Market.Application.Modules.Settings.AppSettings.Queries.Get;

public sealed class GetAppSettingsQueryDto
{
    public required int Id { get; init; }
    public required string BusinessName { get; init; }
    public string? Address { get; init; }
    public string? TaxNumber { get; init; }
    public required int CountryId { get; init; }
    public required string CountryName { get; init; }
    public required decimal VatRate { get; init; }
    public required string DefaultCurrencyCode { get; init; }
    public required RoundingModeType RoundingMode { get; init; }
    public required int ReservationHoldMinutes { get; init; }
}
