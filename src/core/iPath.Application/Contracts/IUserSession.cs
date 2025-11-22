namespace iPath.Application.Contracts;

public interface IUserSession
{
    SessionUserDto? User { get; }
}

public static class UserSessionExtensions
{
    extension(IUserSession session)
    {
        public bool IsAuthenticated => session.User is not null;
        public bool IsAdmin => session.User.roles.Any(r => r.ToLower() == "admin");
        public bool IsModerator => session.User.roles.Any(r => r.ToLower() == "moderator");

        public HashSet<Guid> GroupIds()
        {
            if (session.User is null || session.User.groups is null)
            {
                return [];
            }
            return session.User.groups.Select(g => g.GroupId).ToHashSet();
        }

        public void AssertInGroup(Guid GroupId)
        {
            if (!session.IsAdmin)
            {
                if (!session.User.groups.Any(g => g.GroupId == GroupId))
                {
                    throw new NotAllowedException();
                }
            }
        }

        public void AssertInRole(string Role)
        {
            if (!session.User.roles.Any(r => r.ToLower() == Role.ToLower()))
            {
                throw new NotAllowedException();
            }
        }


        public bool IsGroupModerator(Guid groupId)
            => session.IsAuthenticated && session.User.groups.Any(g => g.GroupId == groupId && g.Role == eMemberRole.Moderator);

        // Admin or user himself
        public bool CanModifyUser(Guid UserId)
            => session.IsAuthenticated && (session.IsAdmin || UserId == session.User.Id);


        public string Username => session.Username;
    }
}