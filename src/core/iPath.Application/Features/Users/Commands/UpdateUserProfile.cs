namespace iPath.Application.Features.Users;

public record UpdateUserProfileCommand(Guid UserId, UserProfile Profile) 
    : IRequest<UpdateUserProfileCommand, Task<Guid>>, IEventInput
{
    public string ObjectName => "User";
}


public class UserProfileUpdatedEvent : EventEntity;
