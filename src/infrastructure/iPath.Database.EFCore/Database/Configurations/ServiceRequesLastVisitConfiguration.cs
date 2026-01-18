namespace iPath_EFCore.Database.Configurations;

internal class ServiceRequesLastVisitConfiguration : IEntityTypeConfiguration<ServiceRequestLastVisit>
{
    public void Configure(EntityTypeBuilder<ServiceRequestLastVisit> b)
    {
        b.ToTable("servicerequest_lastvisits");
        b.HasKey(x => new { x.UserId, x.ServiceRequestId });
        b.HasIndex(x => x.Date);

        b.HasOne(x => x.User).WithMany(u => u.NodeVisitis).HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        b.HasIndex(x => x.ServiceRequestId);
    }
}