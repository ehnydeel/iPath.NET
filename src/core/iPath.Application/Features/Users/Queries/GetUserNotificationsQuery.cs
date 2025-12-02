namespace iPath.Application.Features.Users.Queries;

public record GetUserNotificationsQuery(Guid UserId)
    : IRequest<GetUserNotificationsQuery, Task<IEnumerable<UserGroupNotificationDto>>>;
