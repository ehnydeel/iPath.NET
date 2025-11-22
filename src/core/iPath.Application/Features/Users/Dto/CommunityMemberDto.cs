namespace iPath.Application.Features.Users;

public record CommunityMemberDto(Guid CommunityId, Guid UserId, eMemberRole Role, string? Username = null);
