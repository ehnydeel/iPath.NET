namespace iPath.Application.Features.Users;


public record UpdateCommunityMembershipCommand(Guid UserId, CommunityMemberDto[] Membership) 
    : IRequest<UpdateCommunityMembershipCommand, Task>, IEventInput
{
    public string ObjectName => nameof(User);
}

public class CommunityMembershipUpdatedEvent : EventEntity;
