using Market.Domain.Entities.Shifts;

namespace Market.Infrastructure.Database.Configurations.Shifts;

public class CashRegisterConfiguration : IEntityTypeConfiguration<CashRegisterEntity>
{
    public void Configure(EntityTypeBuilder<CashRegisterEntity> builder)
    {
        builder
            .ToTable("CashRegisters");

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(CashRegisterEntity.Constraints.NameMaxLength);

        builder
            .Property(x => x.IsActive)
            .IsRequired();

        builder
            .HasIndex(x => x.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");
    }
}
