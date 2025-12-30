namespace iPath.Application.Features.Users;

public record DeleteUserCommand (Guid UserId) : IRequest<DeleteUserCommand, Task>;