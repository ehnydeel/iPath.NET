namespace iPath.Application.Features.Users;


// public record UserGroupNotificationDto (Guid GroupId, eNotificationSource Notifications);


public record UserGroupNotificationDto(Guid UserId, Guid GroupId, eNotificationSource Source, eNotificationTarget Tartget, NotificationSettings? Settings = null, string? Groupname = null);


public record UpdateUserNotificationsCommand(Guid UserId, UserGroupNotificationDto[] Notifications) 
    : IRequest<UpdateUserNotificationsCommand, Task>, IEventInput
{
    public string ObjectName => "User";
}


public class UserNotificationsUpdatedEvent : EventEntity;
