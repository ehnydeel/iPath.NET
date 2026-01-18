namespace iPath_EFCore.Database.Configurations;

internal class NodeLastVisitConfiguration : IEntityTypeConfiguration<NodeLastVisit>
{
    public void Configure(EntityTypeBuilder<NodeLastVisit> b)
    {
        b.ToTable("node_lastvisits");
        b.HasKey(x => new { x.UserId, x.NodeId });
        b.HasIndex(x => x.Date);

        b.HasOne(x => x.User).WithMany(u => u.NodeVisitis).HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        b.HasOne(v => v.ServiceRequest).WithMany(n => n.LastVisits).HasForeignKey(v => v.NodeId)
            .OnDelete(DeleteBehavior.NoAction);
        b.HasIndex(x => x.NodeId);
    }
}