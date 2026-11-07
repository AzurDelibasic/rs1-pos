using Market.Domain.Entities.Shifts;

namespace Market.Infrastructure.Database.Configurations.Shifts;

public class ShiftConfiguration : IEntityTypeConfiguration<ShiftEntity>
{
    public void Configure(EntityTypeBuilder<ShiftEntity> builder)
    {
        builder
            .ToTable("Shifts");

        builder
            .Property(x => x.OpenedAtUtc)
            .IsRequired();

        builder
            .Property(x => x.OpeningCash)
            .IsRequired()
            .HasPrecision(ShiftEntity.Constraints.CashPrecision, ShiftEntity.Constraints.CashScale);

        builder
            .Property(x => x.ExpectedCash)
            .HasPrecision(ShiftEntity.Constraints.CashPrecision, ShiftEntity.Constraints.CashScale);

        builder
            .Property(x => x.CountedCash)
            .HasPrecision(ShiftEntity.Constraints.CashPrecision, ShiftEntity.Constraints.CashScale);

        builder
            .Property(x => x.Difference)
            .HasPrecision(ShiftEntity.Constraints.CashPrecision, ShiftEntity.Constraints.CashScale);

        builder
            .Property(x => x.Status)
            .IsRequired();

        builder
            .HasIndex(x => x.CashRegisterId)
            .IsUnique()
            .HasFilter("[Status] = 1 AND [IsDeleted] = 0");

        builder
            .HasIndex(x => x.UserId)
            .IsUnique()
            .HasFilter("[Status] = 1 AND [IsDeleted] = 0");

        builder
            .HasOne(x => x.CashRegister)
            .WithMany(x => x.Shifts)
            .HasForeignKey(x => x.CashRegisterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
