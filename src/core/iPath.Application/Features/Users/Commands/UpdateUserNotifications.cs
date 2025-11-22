namespace iPath.Application.Features.Users;


public record UserGroupNotificationDto (Guid GroupId, eNotification Notifications);


public record UpdateUserNotificationsInput(Guid UserId, UserGroupNotificationDto[] Notifications) 
    : IRequest<UpdateUserNotificationsInput, Task<bool>>, IEventInput
{
    public string ObjectName => "User";
}


public class UserNotificationsUpdatedEvent : EventEntity;
