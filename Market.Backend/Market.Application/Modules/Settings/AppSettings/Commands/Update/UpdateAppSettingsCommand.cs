using Market.Domain.Entities.Settings;

namespace Market.Application.Modules.Settings.AppSettings.Commands.Update;

public sealed class UpdateAppSettingsCommand : IRequest<Unit>
{
    public required string BusinessName { get; set; }
    public string? Address { get; set; }
    public string? TaxNumber { get; set; }
    public required int CountryId { get; set; }
    public required string DefaultCurrencyCode { get; set; }
    public required RoundingModeType RoundingMode { get; set; }
    public required int ReservationHoldMinutes { get; set; }
}
