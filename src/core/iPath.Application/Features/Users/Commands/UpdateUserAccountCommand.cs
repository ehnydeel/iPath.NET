namespace iPath.Application.Features.Users;

public class UpdateUserAccountCommand : IRequest<UpdateUserAccountCommand, Task>
{
    public Guid UserId { get; init; }
    public bool? IsActive { get; set; } = null;
    public UserProfile? Profile { get; set; } = null;
    public string? Username { get; set; } = null;
    public string? Email { get; set; } = null;
    public IEnumerable<Guid>? Roles { get; set; } = null;
}
