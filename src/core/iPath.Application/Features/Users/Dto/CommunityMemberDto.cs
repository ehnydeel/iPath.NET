namespace iPath.Application.Features.Users;

public record CommunityMemberDto(Guid CommunityId, Guid UserId, eMemberRole Role, string? Communityname = null, string? Username = null);
