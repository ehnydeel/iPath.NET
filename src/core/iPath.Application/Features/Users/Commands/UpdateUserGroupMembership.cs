namespace iPath.Application.Features.Users;


public record UpdateGroupMembershipCommand(Guid UserId, UserGroupMemberDto[] Membership) 
    : IRequest<UpdateGroupMembershipCommand, Task>, IEventInput
{
    public string ObjectName => nameof(User);
}

public class GroupMembershipUpdatedEvent : EventEntity;
