namespace iPath_EFCore.Database.Configurations;

internal class CommunityGroupConfiguration : IEntityTypeConfiguration<CommunityGroup>
{
    public void Configure(EntityTypeBuilder<CommunityGroup> b)
    {
        b.ToTable("community_groups");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id");

        b.Property(x => x.CommunityId).HasColumnName("community_id");
        b.HasOne(x => x.Community).WithMany(c => c.ExtraGroups).HasForeignKey(x => x.CommunityId).IsRequired();

        b.Property(x => x.GroupId).HasColumnName("group_id");
        b.HasOne(x => x.Group).WithMany(g => g.ExtraCommunities).HasForeignKey(x => x.GroupId).IsRequired();

        b.HasIndex(builder => new { builder.GroupId, builder.CommunityId }).IsUnique();
    }
}
