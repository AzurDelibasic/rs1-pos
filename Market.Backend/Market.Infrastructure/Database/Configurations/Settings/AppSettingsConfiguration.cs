using Market.Domain.Entities.Settings;

namespace Market.Infrastructure.Database.Configurations.Settings;

public class AppSettingsConfiguration : IEntityTypeConfiguration<AppSettingsEntity>
{
    public void Configure(EntityTypeBuilder<AppSettingsEntity> builder)
    {
        builder
            .ToTable("AppSettings");

        builder
            .Property(x => x.BusinessName)
            .IsRequired()
            .HasMaxLength(AppSettingsEntity.Constraints.BusinessNameMaxLength);

        builder
            .Property(x => x.Address)
            .HasMaxLength(AppSettingsEntity.Constraints.AddressMaxLength);

        builder
            .Property(x => x.TaxNumber)
            .HasMaxLength(AppSettingsEntity.Constraints.TaxNumberMaxLength);

        builder
            .Property(x => x.RoundingMode)
            .IsRequired();

        builder
            .Property(x => x.ReservationHoldMinutes)
            .IsRequired();

        builder
            .HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.DefaultCurrency)
            .WithMany()
            .HasForeignKey(x => x.DefaultCurrencyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
