using System.Diagnostics;

namespace iPath.Domain.Entities;

[DebuggerDisplay("g={GroupId}, u={UserId}")]
public class GroupMember : BaseEntity
{
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public eMemberRole Role { get; set; }


    // User Preferences
    public bool IsFavourite { get; set; }

    public eNotificationSource Notifications { get; set; } = eNotificationSource.None;
    public eNotificationTarget NotificationTargets { get; set; } = eNotificationTarget.None;
    public NodeFilter? NotificationFilter { get; set; }
}



[Flags]
public enum eNotificationSource
{
    None = 0,
    NewCase = 1,
    NewAnnotation = 2,
    NewAnnotationOnMyCase = 4
}


public class NodeFilter
{
    public string? IcdoTopoCode { get; set; }
}