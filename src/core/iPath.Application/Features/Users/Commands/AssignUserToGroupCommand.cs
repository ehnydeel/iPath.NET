namespace iPath.Application.Features.Users;

public record AssignUserToGroupCommand(Guid userId, Guid groupId)
    : IRequest<AssignUserToGroupCommand, Task>;
