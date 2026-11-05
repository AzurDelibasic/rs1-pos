using Market.Domain.Common;

namespace Market.Domain.Entities.Shifts;

public class CashRegisterEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public static class Constraints
    {
        public const int NameMaxLength = 50;
    }
}
