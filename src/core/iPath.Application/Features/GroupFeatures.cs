using System.ComponentModel.DataAnnotations;

namespace iPath.Application.Features;

public record GroupListDto(Guid Id, string Name, int? TotalNodes = null, int? NewNodes = null, int? NewAnnotation = null);
public record GroupDto(Guid Id, string Name, GroupSettings Settings, GroupMemberDto[]? Members);

public record UserGroupMemberDto(Guid GroupId, string Groupname, eMemberRole Role);

public record GroupMemberDto(Guid UserId, string Username, eMemberRole Role);



public interface IGroupService
{
    Task<PagedResultList<GroupListDto>> GetGroupListAsync(GetGroupListQuery query, CancellationToken ct = default);
    Task<GroupDto> GetGroupByIdAsync(Guid GroupId, CancellationToken ct = default);


    Task<GroupDto> CreateGroupAsync(CreateGroupCommand cmd, CancellationToken ct = default);
    Task UpdateGroupAsync(UpdateGroupCommand cmd, CancellationToken ct = default);
    Task DeleteGroupAsync(DeleteGroupCommand  cmd, CancellationToken ct = default);


    Task<GroupAssignedToCommunityEvent> AssignGroupToCommunityAsync(AssignGroupToCommunityCommand cmd, CancellationToken ct = default);
    Task AssignQuestionnaireToGroupAsync(AssignQuestionnaireToGroupCommand cmd, CancellationToken ct = default);
}





public class GetGroupListQuery : PagedQuery<GroupListDto>
    , IRequest<GetGroupListQuery, Task<PagedResultList<GroupListDto>>>
{
    public bool IncludeCounts { get; set; }
    public bool AdminList { get; set; }
    public string SearchString { get; set; }
}

public record GetGroupByIdQuery(Guid GroupId) : IRequest<GetGroupByIdQuery, Task<GroupDto>>;

public record AssignGroupToCommunityCommand(Guid GroupId, Guid CommunityId, bool Remove = false)
    : IRequest<AssignGroupToCommunityCommand, Task<GroupAssignedToCommunityEvent>>, IEventInput
{
    public string ObjectName => nameof(Group);
}


public record AssignQuestionnaireToGroupCommand(Guid Id, Guid GroupId, eQuestionnaireUsage Usage)
    : IRequest<AssignQuestionnaireToGroupCommand, Task<QuestionnaireAssignedToGroupEvent>>
    , IEventInput
{
    public string ObjectName => nameof(Group);
}


public record CreateGroupCommand : IRequest<CreateGroupCommand, Task<GroupDto>>, IEventInput
{
    [MinLength(4)]
    public string Name { get; init; }
    public GroupSettings Settings { get; init; }
    public eGroupVisibility? Visibility { get; init; } = null;

    public Guid OwnerId { get; init; }
    public Guid? CommunityId { get; init; } = null;

    public string ObjectName => nameof(Group);
}

public record DeleteGroupCommand(Guid Id) : IRequest<DeleteGroupCommand, Task<Guid>>, IEventInput
{
    public string ObjectName => nameof(Group);
}

public class UpdateGroupCommand : IRequest<UpdateGroupCommand, Task>, IEventInput
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public GroupSettings? Settings { get; set; } = null;

    public Guid? OwnerId { get; set; }

    public string ObjectName => nameof(Group);
}


public class GroupAssignedToCommunityEvent : EventEntity;
public class QuestionnaireAssignedToGroupEvent : EventEntity { }

public class GroupCreatedEvent : EventEntity;
public class GroupDeletedEvent : EventEntity;
public class GroupUpdatedEvent : EventEntity;