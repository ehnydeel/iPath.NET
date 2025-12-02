namespace iPath.Application.Features.Users;

public record GetUserNotificationsQuery(Guid UserId)
    : IRequest<GetUserNotificationsQuery, Task<IEnumerable<UserGroupNotificationDto>>>;
