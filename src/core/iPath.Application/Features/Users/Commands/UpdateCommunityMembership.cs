namespace iPath.Application.Features.Users;


public record UpdateCommunityMembershipInput(Guid UserId, CommunityMemberDto[] Membership) 
    : IRequest<UpdateCommunityMembershipInput, Task>, IEventInput
{
    public string ObjectName => nameof(User);
}

public class CommunityMembershipUpdatedEvent : EventEntity;
