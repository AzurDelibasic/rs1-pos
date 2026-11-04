using Market.Domain.Entities.Venue;

namespace Market.Infrastructure.Database.Configurations.Venue;

public class TableConfiguration : IEntityTypeConfiguration<TableEntity>
{
    public void Configure(EntityTypeBuilder<TableEntity> builder)
    {
        builder
            .ToTable("Tables");

        builder
            .Property(x => x.Label)
            .IsRequired()
            .HasMaxLength(TableEntity.Constraints.LabelMaxLength);

        builder
            .Property(x => x.Seats)
            .IsRequired();

        builder
            .Property(x => x.IsActive)
            .IsRequired();

        builder
            .HasIndex(x => new { x.RegionId, x.Label })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder
            .HasOne(x => x.Region)
            .WithMany(x => x.Tables)
            .HasForeignKey(x => x.RegionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
