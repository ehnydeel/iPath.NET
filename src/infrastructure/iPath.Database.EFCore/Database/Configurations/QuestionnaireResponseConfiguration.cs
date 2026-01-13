namespace iPath_EFCore.Database.Configurations;

internal class QuestionnaireResponseConfiguration : IEntityTypeConfiguration<QuestionnaireResponseEntity>
{
    public void Configure(EntityTypeBuilder<QuestionnaireResponseEntity> b)
    {
        b.ToTable("questionnaire_responses");
        b.HasKey(r => r.Id);
        b.Property(x => x.Id).HasColumnName("id");

        b.HasOne(r => r.Questionnaire)
            .WithMany()
            .HasForeignKey(r => r.QuestionnaireId)
            .IsRequired(true);

        b.HasOne(q => q.Owner)
            .WithMany()
            .IsRequired(true)
            .OnDelete(DeleteBehavior.NoAction);

        b.HasOne(r => r.Node)
            .WithMany(n => n.QuestionnaireResponses)
            .HasForeignKey(r => r.NodeId)
            .IsRequired(false);

        b.HasOne(r => r.Annotation)
            .WithMany(a => a.QuestionnaireResponses)
            .HasForeignKey(r => r.AnnotationId)
            .IsRequired(false);
        
        // b.Property(r => r.Resource).HasColumnType("jsonb");
    }
}
