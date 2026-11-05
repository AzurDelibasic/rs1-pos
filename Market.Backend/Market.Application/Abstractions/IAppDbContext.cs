using Market.Domain.Entities.Finance;
using Market.Domain.Entities.Shifts;
using Market.Domain.Entities.Venue;

namespace Market.Application.Abstractions;

// Application layer
public interface IAppDbContext
{
    DbSet<ProductEntity> Products { get; }
    DbSet<ProductCategoryEntity> ProductCategories { get; }
    DbSet<MarketUserEntity> Users { get; }
    DbSet<RefreshTokenEntity> RefreshTokens { get; }
    DbSet<PasswordResetTokenEntity> PasswordResetTokens { get; }

    DbSet<CurrencyEntity> Currencies { get; }
    DbSet<ExchangeRateEntity> ExchangeRates { get; }

    DbSet<RegionEntity> Regions { get; }
    DbSet<TableEntity> Tables { get; }

    DbSet<CashRegisterEntity> CashRegisters { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}
