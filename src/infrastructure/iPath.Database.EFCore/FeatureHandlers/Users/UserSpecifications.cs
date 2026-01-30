using System.Linq.Expressions;

namespace iPath.EF.Core.FeatureHandlers.Users;


public class UserHasIdSpecifications(Guid UserId) : Specification<User>
{
    public override Expression<Func<User, bool>> ToExpression()
    {
        return u => u.Id == UserId;
    }
}

public class UserIsGroupMemberSpecifications(Guid GroupId) : Specification<User>
{
    public override Expression<Func<User, bool>> ToExpression()
    {
        return u => u.GroupMembership.Any(m => m.GroupId == GroupId && m.Role != eMemberRole.Banned);
    }
}

public class UserIsCommunityMemberSpecifications(Guid CommunityId) : Specification<User>
{
    public override Expression<Func<User, bool>> ToExpression()
    {
        return u => u.CommunityMembership.Any(m => m.CommunityId == CommunityId && m.Role != eMemberRole.Banned);
    }
}