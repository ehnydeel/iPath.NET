namespace iPath.Application.Features.ServiceRequests;

public record DeleteAnnotationCommand(Guid AnnotationId)
    : IRequest<DeleteAnnotationCommand, Task<Guid>>
    , IEventInput
{
    public string ObjectName => nameof(ServiceRequest);
}

