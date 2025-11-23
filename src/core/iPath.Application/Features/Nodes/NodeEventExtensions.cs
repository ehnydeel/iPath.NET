using DispatchR.Abstractions.Send;
using System.Text.Json;

namespace iPath.Application.Features.Nodes;

public static class NodeEventExtensions
{
    internal static EventEntity CreateEvent<TEvent, TInput>(this Node node,
        TInput input,
        Guid? userId = null,
        CancellationToken ct = default)
        where TEvent : NodeEvent, new()
        where TInput : IEventInput
    {
        var e = new TEvent
        {
            EventId = Guid.CreateVersion7(),
            EventDate = DateTime.UtcNow,
            UserId = userId,
            EventName = typeof(TEvent).Name,
            ObjectName = input.ObjectName,
            ObjectId = node.Id,
            GroupId = node.GroupId,
            Payload = JsonSerializer.Serialize(input),
        };
        node.AddEventEntity(e);
        return e;
    }
}

public static class CreateNodeExtensions
{
    public static Node CreateNode(CreateNodeCommand cmd, Guid userId)
    {
        var node = new Node
        {
            Id = cmd.NodeId.HasValue ? cmd.NodeId.Value : Guid.CreateVersion7(),
            CreatedOn = DateTime.UtcNow,
            LastModifiedOn = DateTime.UtcNow,
            GroupId = cmd.GroupId,
            OwnerId = userId,
            Description = cmd.Description ?? new(),
            NodeType = cmd.NodeType,
            IsDraft = true
        };

        node.CreateEvent<RootNodeCreatedEvent, CreateNodeCommand>(cmd, userId);
        return node;
    }




}
