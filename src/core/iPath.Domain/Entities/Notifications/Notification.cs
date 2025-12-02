namespace iPath.Domain.Entities;

public class Notification : BaseEntity
{
    public DateTime CreatedOn { get; private set; }
    public DateTime? ProcessedOn { get; private set; }
    public NotificationStatus Status { get; private set; } = NotificationStatus.Pending;

    public Guid UserId {  get; private set; }
    public User User { get; private set; }

    public eNotificationTarget Type { get; private set; } = eNotificationTarget.None;

    public string Data { get; private set; }
    public string? ErrorMessage { get; private set; }

    private Notification()
    {        
    }

    public static Notification Create(eNotificationTarget type, string data)
    {
        return new Notification
        {
            Id = Guid.CreateVersion7(),
            CreatedOn = DateTime.UtcNow,
            Type = type,
            Status = NotificationStatus.Pending,
            Data = data
        };
    }

    public Notification MarkAsSent()
    {
        Status = NotificationStatus.Sent;
        ProcessedOn = DateTime.UtcNow;
        return this;
    }

    public Notification MarkAsFailed(string errorMessage)
    {
        Status = NotificationStatus.Failed;
        ProcessedOn = DateTime.UtcNow;
        ErrorMessage = errorMessage;
        return this;
    }

    public Notification UpdateStatus(NotificationStatus status)
    {
        Status = status;
        return this;
    }
}


[Flags]
public enum eNotificationTarget
{
    None = 0,
    InApp = 1,
    Email = 2
}



public enum NotificationStatus
{
    Pending = 0,
    Sent = 1,
    Failed = 2
}
