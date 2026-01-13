namespace iPath_EFCore.Database.Configurations;

internal class NodeImportConfiguration : IEntityTypeConfiguration<NodeImport>
{
    public void Configure(EntityTypeBuilder<NodeImport> b)
    {
        b.HasKey(i => i.NodeId);
    }
}
