using Market.Domain.Common;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities.Shifts;

public class ShiftEntity : BaseEntity
{
    public int CashRegisterId { get; set; }

    public CashRegisterEntity? CashRegister { get; set; }

    public int UserId { get; set; }

    public MarketUserEntity? User { get; set; }

    public DateTime OpenedAtUtc { get; set; }

    public DateTime? ClosedAtUtc { get; set; }

    public decimal OpeningCash { get; set; }

    public decimal? ExpectedCash { get; set; }

    public decimal? CountedCash { get; set; }

    public decimal? Difference { get; set; }

    public ShiftStatusType Status { get; set; } = ShiftStatusType.Open;

    public static class Constraints
    {
        public const int CashPrecision = 18;
        public const int CashScale = 2;
        public const decimal CashMax = 1_000_000m;
    }
}
