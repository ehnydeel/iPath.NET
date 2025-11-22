using System.Diagnostics;

namespace iPath.Application.Features.Users;

[DebuggerDisplay("{GroupId} => {nNtifications}")]
public record GroupNotificationDto(Guid GroupId, Guid UserId, eNotification Notifications);