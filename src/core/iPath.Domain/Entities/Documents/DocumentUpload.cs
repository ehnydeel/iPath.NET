namespace iPath.Domain.Entities;

public class DocumentUpload
{
    public Guid Id { get; set; }
    public Guid NodeId { get; set; }
    public DateTime ScheduledDate { get; set; }
}