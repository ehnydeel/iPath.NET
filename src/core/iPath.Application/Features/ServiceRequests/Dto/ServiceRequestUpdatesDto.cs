namespace iPath.Application.Features.ServiceRequests;

public class ServiceRequestUpdatesDto
{
    public List<ServiceRequestIds> NewRequests { get; set; } = [];
    public List<ServiceRequestIds> NewAnnotations { get; set; } = [];
}

public record ServiceRequestIds(Guid Id, Guid GroupId, string? BodySiteCode);