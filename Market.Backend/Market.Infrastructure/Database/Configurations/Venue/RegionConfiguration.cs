using Market.Domain.Entities.Venue;

namespace Market.Infrastructure.Database.Configurations.Venue
{
    public class RegionConfiguration : IEntityTypeConfiguration<RegionEntity>
    {
        public void Configure(EntityTypeBuilder<RegionEntity> builder)
        {
            builder
                .ToTable("Regions");

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(RegionEntity.Constraints.NameMaxLength);

            builder
                .Property(x => x.SortOrder)
                .IsRequired();

            builder
                .Property(x => x.IsActive)
                .IsRequired();

            builder
                .HasIndex(x => x.Name)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");
        }
    }
}
