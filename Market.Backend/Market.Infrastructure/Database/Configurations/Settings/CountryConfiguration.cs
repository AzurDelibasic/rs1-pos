using Market.Domain.Entities.Settings;

namespace Market.Infrastructure.Database.Configurations.Settings;

public class CountryConfiguration : IEntityTypeConfiguration<CountryEntity>
{
    public void Configure(EntityTypeBuilder<CountryEntity> builder)
    {
        builder
            .ToTable("Countries");

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(CountryEntity.Constraints.NameMaxLength);

        builder
            .Property(x => x.Code)
            .IsRequired()
            .IsFixedLength()
            .HasMaxLength(CountryEntity.Constraints.CodeLength);

        builder
            .Property(x => x.VatRate)
            .IsRequired()
            .HasPrecision(CountryEntity.Constraints.VatRatePrecision, CountryEntity.Constraints.VatRateScale);

        builder
            .HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder
            .HasIndex(x => x.Code)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}
