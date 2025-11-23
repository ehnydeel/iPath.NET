using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace iPath.EF.Core.FeatureHandlers.Users.Commands;

public class UpdateUsersRoleHandler(UserManager<User> um, RoleManager<Role> rm, IUserSession sess, ILogger<UpdateUserRoleHandler> logger)
    : IRequestHandler<UpdateUserRolesCommand, Task>
{
    public async Task Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        var user = await um.FindByIdAsync(request.UserId.ToString());
        Guard.Against.NotFound(request.UserId, user);

        foreach(var role in await rm.Roles.ToListAsync(cancellationToken))
        {
            if (request.Roles.Contains(role.Id))
            {
                // add role
                await um.AddToRoleAsync(user, role.Name);
            }
            else
            {
                // remove role
                await um.RemoveFromRoleAsync(user, role.Name);
            }
        }
    }
}