
using iPath.Application.Features;

namespace iPath.Application.Contracts;

public interface IGroupCache
{
    Task ClearGroup(Guid Id);
    Task<GroupDto?> GetGroupAsync(Guid Id);
}