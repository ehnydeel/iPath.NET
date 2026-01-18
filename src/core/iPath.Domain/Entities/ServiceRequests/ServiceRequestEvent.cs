namespace iPath.Domain.Entities;

/*
public interface INodeEvent
{
    Node Node { get; }
}
*/


public class ServiceRequestEvent : EventEntity //, INodeEvent
{
    [JsonIgnore]
    public ServiceRequest ServiceRequest { get; set; }
}

