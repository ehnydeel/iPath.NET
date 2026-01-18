namespace iPath_EFCore.Database.Configurations;

internal class NodeImportConfiguration : IEntityTypeConfiguration<NodeImport>
{
    public void Configure(EntityTypeBuilder<NodeImport> b)
    {
        b.ToTable("node_data_import");
        b.HasKey(i => i.NodeId);
    }
}
