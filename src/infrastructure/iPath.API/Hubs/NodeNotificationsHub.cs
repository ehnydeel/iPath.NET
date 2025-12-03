using Microsoft.AspNetCore.SignalR;

namespace iPath.API.Hubs;

public class NodeNotificationsHub : Hub
{
    public async Task SendNotification(NotificationMessage e){
        await Clients.All.SendAsync("NodeEvent", e);
    }
}


