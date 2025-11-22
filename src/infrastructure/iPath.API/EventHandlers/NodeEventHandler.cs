using DispatchR.Abstractions.Notification;
using iPath.API.Hubs;
using iPath.Domain.Notificxations;
using Microsoft.AspNetCore.SignalR;
using Scalar.AspNetCore;

namespace iPath.API;


public class RootNodeCreatedEventHandler(IHubContext<NodeNotificationsHub> hub, IUserSession sess) 
    : INotificationHandler<RootNodeCreatedEvent>
{
    public async ValueTask Handle(RootNodeCreatedEvent evt, CancellationToken cancellationToken)
    {
        var n = new NodeNofitication(GroupId: evt.GroupId.Value, NodeId: evt.ObjectId, 
            OwnerId: sess.User.Id, EventDate: DateTime.UtcNow, 
            type: eNodeEventType.NewNode, "new node");
        await hub.Clients.All.SendAsync("NodeEvent", n);
    }
}


public class RootNodePublishedEventHandler(IHubContext<NodeNotificationsHub> hub, IUserSession sess)
    : INotificationHandler<RootNodePublishedEvent>
{
    public async ValueTask Handle(RootNodePublishedEvent evt, CancellationToken cancellationToken)
    {
        var n = new NodeNofitication(GroupId: evt.GroupId.Value, NodeId: evt.ObjectId,
            OwnerId: sess.User.Id, EventDate: DateTime.UtcNow,
            type: eNodeEventType.NodePublished, "node published");
        await hub.Clients.All.SendAsync("NodeEvent", n);
    }
}

public class AnnotationCreatedEventHandler(IHubContext<NodeNotificationsHub> hub, IUserSession sess)
    : INotificationHandler<AnnotationCreatedEvent>
{
    public async ValueTask Handle(AnnotationCreatedEvent evt, CancellationToken cancellationToken)
    {
        var n = new NodeNofitication(GroupId: evt.GroupId, NodeId: evt.ObjectId,
            OwnerId: sess.User.Id, EventDate: DateTime.UtcNow,
            type: eNodeEventType.NewAnnotation, "new annotation");
        await hub.Clients.All.SendAsync("NodeEvent", n);
    }
}