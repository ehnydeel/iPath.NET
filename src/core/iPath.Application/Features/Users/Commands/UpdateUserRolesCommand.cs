namespace iPath.Application.Features.Users;

public record UpdateUserRolesCommand(Guid UserId, IEnumerable<Guid> Roles)
    : IRequest<UpdateUserRolesCommand, Task>;
