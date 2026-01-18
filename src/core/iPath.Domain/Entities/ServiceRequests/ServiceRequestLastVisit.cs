namespace iPath.Domain.Entities;


public class ServiceRequestLastVisit : IEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid ServiceRequestId { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    private ServiceRequestLastVisit()
    {
    }

    public static ServiceRequestLastVisit Create(Guid userId, Guid nodeId, DateTime date)
    {
        return new ServiceRequestLastVisit { UserId = userId, ServiceRequestId = nodeId, Date = date };
    }
}
